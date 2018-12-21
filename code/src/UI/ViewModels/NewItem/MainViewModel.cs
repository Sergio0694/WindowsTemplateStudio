﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Services;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.Common;
using Microsoft.Templates.UI.Views.NewItem;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        private RelayCommand _refreshTemplatesCacheCommand;

        private static NewItemGenerationResult _output;

        private GenerationService _generationService = GenerationService.Instance;

        public TemplateType TemplateType { get; set; }

        public string ConfigPlatform { get; private set; }

        public string ConfigFramework { get; private set; }

        public string ConfigProjectType { get; private set; }

        public static MainViewModel Current { get; private set; }

        public TemplateSelectionViewModel TemplateSelection { get; } = new TemplateSelectionViewModel();

        public ObservableCollection<BreakingChangeMessageViewModel> BreakingChangesErrors { get; set; } = new ObservableCollection<BreakingChangeMessageViewModel>();

        public ChangesSummaryViewModel ChangesSummary { get; } = new ChangesSummaryViewModel();

        public RelayCommand RefreshTemplatesCacheCommand => _refreshTemplatesCacheCommand ?? (_refreshTemplatesCacheCommand = new RelayCommand(
            () => SafeThreading.JoinableTaskFactory.RunAsync(async () => await OnRefreshTemplatesAsync())));

        private static IEnumerable<Step> NewItemSteps
        {
            get
            {
                yield return new Step(0, StringRes.NewItemStepOne, () => new TemplateSelectionPage(), true, true);
                yield return new Step(1, StringRes.NewItemStepTwo, () => new ChangesSummaryPage(_output));
            }
        }

        public MainViewModel(WizardShell mainWindow, BaseStyleValuesProvider provider)
            : base(mainWindow, provider, NewItemSteps, false)
        {
            Current = this;
            Navigation.OnFinish += OnFinish;
            Navigation.OnStepUpdated += OnStepUpdated;
            Navigation.IsStepAvailable = IsStepAvailableAsync;
        }

        public async Task InitializeAsync(TemplateType templateType, string language)
        {
            TemplateType = templateType;
            var stringResource = templateType == TemplateType.Page ? StringRes.NewItemTitlePage : StringRes.NewItemTitleFeature;
            WizardStatus.Title = stringResource;
            SetProjectConfigInfo();
            await InitializeAsync(ConfigPlatform, language);
        }

        public override void UnsubscribeEventHandlers()
        {
            base.UnsubscribeEventHandlers();
            Navigation.OnFinish -= OnFinish;
        }

        public override void ProcessItem(object item)
        {
            if (item is TemplateInfoViewModel template)
            {
                TemplateSelection.SelectTemplate(template);
            }
        }

        public override bool IsSelectionEnabled(MetadataType metadataType) => true;

        protected override async Task OnTemplatesAvailableAsync()
        {
            TemplateSelection.LoadData(TemplateType, ConfigFramework, ConfigPlatform);
            WizardStatus.IsLoading = false;

            var result = BreakingChangesValidatorService.Validate();
            if (!result.IsValid)
            {
                var messages = result.ErrorMessages.Select(e => new BreakingChangeMessageViewModel(e));
                BreakingChangesErrors.AddRange(messages);
                OnPropertyChanged(nameof(BreakingChangesErrors));

                await Task.CompletedTask;
            }
        }

        protected async Task OnRefreshTemplatesAsync()
        {
            try
            {
                WizardStatus.IsLoading = true;
                await GenContext.ToolBox.Repo.RefreshAsync(true);
            }
            catch (Exception ex)
            {
                await NotificationsControl.AddNotificationAsync(Notification.Error(StringRes.NotificationSyncError_Refresh));

                await AppHealth.Current.Error.TrackAsync(ex.ToString());
                await AppHealth.Current.Exception.TrackAsync(ex);
            }
            finally
            {
                WizardStatus.IsLoading = GenContext.ToolBox.Repo.SyncInProgress;
            }
        }

        private async Task<bool> IsStepAvailableAsync(Step step)
        {
            if (step.Index == 1 && !WizardStatus.HasValidationErrors)
            {
                _output = await CleanupAndGenerateNewItemAsync();
                if (!_output.HasChangesToApply)
                {
                    var message = TemplateType == TemplateType.Page ? StringRes.NewItemHasNoChangesPage : StringRes.NewItemHasNoChangesFeature;
                    message = string.Format(message, TemplateSelection.Name);
                    var notification = Notification.Warning(message, Category.RightClickItemHasNoChanges);
                    NotificationsControl.AddNotificationAsync(notification).FireAndForget();
                }

                return _output.HasChangesToApply;
            }

            return true;
        }

        private void OnStepUpdated(object sender, Step step)
        {
            if (step.Index == 0)
            {
                ChangesSummary.ClearSelected();
            }
        }

        private void OnFinish(object sender, EventArgs e)
        {
            var userSelection = new UserSelection(ConfigProjectType, ConfigFramework, ConfigPlatform, Language);
            userSelection.Add((TemplateSelection.Name, TemplateSelection.Template));
            WizardShell.Current.Result = userSelection;
            WizardShell.Current.Result.ItemGenerationType = ChangesSummary.DoNotMerge ? ItemGenerationType.Generate : ItemGenerationType.GenerateAndMerge;
        }

        private async Task<NewItemGenerationResult> CleanupAndGenerateNewItemAsync()
        {
            NewItemGenController.Instance.CleanupTempGeneration();
            var userSelection = CreateUserSelection();
            await _generationService.GenerateNewItemAsync(TemplateSelection.Template.GetTemplateType(), userSelection);
            return NewItemGenController.Instance.CompareOutputAndProject();
        }

        private UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection(ConfigProjectType, ConfigFramework, ConfigPlatform, Language) { HomeName = string.Empty };
            var dependencies = GenComposer.GetAllDependencies(TemplateSelection.Template, ConfigFramework, ConfigPlatform);
            userSelection.Add((TemplateSelection.Name, TemplateSelection.Template));
            foreach (var dependencyTemplate in dependencies)
            {
                userSelection.Add((dependencyTemplate.GetDefaultName(), dependencyTemplate));
            }

            return userSelection;
        }

        private void SetProjectConfigInfo()
        {
            var configInfo = ProjectConfigInfo.ReadProjectConfiguration();
            if (string.IsNullOrEmpty(configInfo.ProjectType) || string.IsNullOrEmpty(configInfo.Framework) || string.IsNullOrEmpty(configInfo.Platform))
            {
                var vm = new ProjectConfigurationViewModel();
                ProjectConfigurationDialog projectConfig = new ProjectConfigurationDialog(vm);
                projectConfig.Owner = WizardShell.Current;
                projectConfig.ShowDialog();

                if (vm.Result == DialogResult.Accept)
                {
                    configInfo.ProjectType = vm.SelectedProjectType.Name;
                    configInfo.Framework = vm.SelectedFramework.Name;
                    configInfo.Platform = vm.SelectedPlatform;
                    ProjectMetadataService.SaveProjectMetadata(configInfo);
                    ConfigFramework = configInfo.Framework;
                    ConfigProjectType = configInfo.ProjectType;
                    ConfigPlatform = configInfo.Platform;
                }
                else
                {
                    Navigation.Cancel();
                }
            }
            else
            {
                ConfigFramework = configInfo.Framework;
                ConfigProjectType = configInfo.ProjectType;
                ConfigPlatform = configInfo.Platform;
            }
        }
    }
}
