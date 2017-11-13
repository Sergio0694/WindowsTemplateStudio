﻿using System;

using DragAndDropExample.Helpers;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using DragAndDropExample.Models;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.ViewModels
{
    public class Scenario4ViewModel : Observable
    {

        public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();

        private ICommand _getStorageItemsCommand;
        private ICommand _dragEnterCommand;
        private ICommand _dragLeaveCommand;

        public ICommand GetStorageItemsCommand => _getStorageItemsCommand ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));
        public ICommand DragEnterCommand => _dragEnterCommand ?? (_dragEnterCommand = new RelayCommand<DragDropData>(OnDragEnter));
        public ICommand DragLeaveCommand => _dragLeaveCommand ?? (_dragLeaveCommand = new RelayCommand<DragDropData>(OnDragLeave));

        public Scenario4ViewModel()
        {
        }

        private async void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                Items.Add(await CustomItemFactory.Create(item));
            }

            ShowDragMask = false;
        }

        private void OnDragEnter(DragDropData overData)
        {
            ShowDragMask = true;
        }
        
        private void OnDragLeave(DragDropData overData)
        {
            ShowDragMask = false;
        }

        private bool _showDragMask;
        public bool ShowDragMask
        {
            get => _showDragMask;
            set => Set(ref _showDragMask, value);
        }
    }
}
