﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Navigation;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ContentGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;
        private readonly IConnectedAnimationService _connectedAnimationService;

        private ObservableCollection<SampleOrder> _source;
        private ICommand _itemClickCommand;

        public ObservableCollection<SampleOrder> Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new DelegateCommand<SampleOrder>(OnItemClick));

        public ContentGridViewViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance, IConnectedAnimationService connectedAnimationService)
        {
            _navigationService = navigationServiceInstance;
            _sampleDataService = sampleDataServiceInstance;
            _connectedAnimationService = connectedAnimationService;

            // TODO WTS: Replace this with your actual data
            Source = _sampleDataService.GetContentGridData();
        }

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                _connectedAnimationService.SetListDataItemForNextConnectedAnnimation(clickedItem);
                _navigationService.Navigate(PageTokens.ContentGridViewDetailPage, clickedItem.OrderId);
            }
        }
    }
}
