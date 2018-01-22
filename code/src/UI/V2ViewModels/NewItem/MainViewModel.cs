﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Generation;
using Microsoft.Templates.UI.Threading;
using Microsoft.Templates.UI.V2Controls;
using Microsoft.Templates.UI.V2Resources;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewItem;

namespace Microsoft.Templates.UI.V2ViewModels.NewItem
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;

        private NewItemGenerationResult _output;

        public TemplateType TemplateType;

        public string Language { get; private set; }

        public string ConfigFramework { get; private set; }

        public string ConfigProjectType { get; private set; }

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel(WizardShell.Current));

        public TemplateSelectionViewModel TemplateSelection { get; } = new TemplateSelectionViewModel();

        public ChangesSummaryViewModel ChangesSummary { get; } = new ChangesSummaryViewModel();

        public MainViewModel(WizardShell mainWindow)
            : base(mainWindow)
        {
        }

        public async Task InitializeAsync(TemplateType templateType, string language)
        {
            TemplateType = templateType;
            Language = language;
            var stringResource = templateType == TemplateType.Page ? StringRes.NewItemTitlePage : StringRes.NewItemTitleFeature;
            WizardStatus.Title = stringResource;
            await InitializeAsync(language);
        }

        protected override async Task<bool> IsStepAvailableAsync(int step)
        {
            if (step == 1)
            {
                _output = await CleanupAndGenerateNewItemAsync();
                if (!_output.HasChangesToApply)
                {
                    var message = TemplateType == TemplateType.Page ? StringRes.NewItemHasNoChangesPage : StringRes.NewItemHasNoChangesFeature;
                    message = string.Format(message, TemplateSelection.Name);
                    var notification = Notification.Warning(message, Category.RightClickItemHasNoChanges);
                    NotificationsControl.Instance.AddNotificationAsync(notification).FireAndForget();
                }

                return _output.HasChangesToApply;
            }

            return await base.IsStepAvailableAsync(step);
        }

        protected override void UpdateStep()
        {
            base.UpdateStep();
            if (Step == 0)
            {
                NavigationService.NavigateSecondaryFrame(new TemplateSelectionPage());
            }
            else if (Step == 1)
            {
                NavigationService.NavigateSecondaryFrame(new ChangesSummaryPage(_output));
            }

            SetCanGoBack(Step > 0);
            SetCanGoForward(Step < 1);
        }

        private async Task<NewItemGenerationResult> CleanupAndGenerateNewItemAsync()
        {
            NewItemGenController.Instance.CleanupTempGeneration();
            var userSelection = CreateUserSelection();
            await NewItemGenController.Instance.GenerateNewItemAsync(TemplateSelection.Template.GetTemplateType(), userSelection);
            return NewItemGenController.Instance.CompareOutputAndProject();
        }

        private UserSelection CreateUserSelection()
        {
            var userSelection = new UserSelection(ConfigProjectType, ConfigFramework, Language) { HomeName = string.Empty };
            var dependencies = GenComposer.GetAllDependencies(TemplateSelection.Template, ConfigFramework);
            userSelection.Add((TemplateSelection.Name, TemplateSelection.Template));
            foreach (var dependencyTemplate in dependencies)
            {
                userSelection.Add((dependencyTemplate.GetDefaultName(), dependencyTemplate));
            }

            return userSelection;
        }

        protected override void OnCancel() => WizardShell.Current.Close();

        protected override Task OnTemplatesAvailableAsync()
        {
            SetProjectConfigInfo();
            TemplateSelection.LoadData(TemplateType, ConfigFramework);
            WizardStatus.IsLoading = false;
            return Task.CompletedTask;
        }

        private void SetProjectConfigInfo()
        {
            var configInfo = ProjectConfigInfo.ReadProjectConfiguration();
            if (string.IsNullOrEmpty(configInfo.ProjectType) || string.IsNullOrEmpty(configInfo.Framework))
            {
                // TODO: mvegaca
                //WizardStatus.InfoShapeVisibility = Visibility.Visible;
                //ProjectConfigurationWindow projectConfig = new ProjectConfigurationWindow(MainView);

                //if (projectConfig.ShowDialog().Value)
                //{
                //    configInfo.ProjectType = projectConfig.ViewModel.SelectedProjectType.Name;
                //    configInfo.Framework = projectConfig.ViewModel.SelectedFramework.Name;
                //    WizardStatus.InfoShapeVisibility = Visibility.Collapsed;
                //}
                //else
                //{
                //    Cancel();
                //}
            }

            ConfigFramework = configInfo.Framework;
            ConfigProjectType = configInfo.ProjectType;
        }

        protected override IEnumerable<Step> GetSteps()
        {
            yield return new Step(0, StringRes.NewItemStepOne, true);
            yield return new Step(1, StringRes.NewItemStepTwo);
        }
    }
}