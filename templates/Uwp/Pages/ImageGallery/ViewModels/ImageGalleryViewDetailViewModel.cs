﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Windows.UI.Xaml.Navigation;

namespace Param_RootNamespace.ViewModels
{
    public class ImageGalleryViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private object _selectedImage;
        private ObservableCollection<SampleImage> _source;

        public object SelectedImage
        {
            get => _selectedImage;
            set
            {
                Param_Setter(ref _selectedImage, value);
                ImagesNavigationHelper.UpdateImageId(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey, ((SampleImage)SelectedImage).ID);
            }
        }

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Param_Setter(ref _source, value);
        }

        public ImageGalleryViewDetailViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
        }

        public void Initialize(string selectedImageID, NavigationMode navigationMode)
        {
            if (!string.IsNullOrEmpty(selectedImageID) && navigationMode == NavigationMode.New)
            {
                SelectedImage = Source.FirstOrDefault(i => i.ID == selectedImageID);
            }
            else
            {
                selectedImageID = ImagesNavigationHelper.GetImageId(ImageGalleryViewViewModel.ImageGalleryViewSelectedIdKey);
                if (!string.IsNullOrEmpty(selectedImageID))
                {
                    SelectedImage = Source.FirstOrDefault(i => i.ID == selectedImageID);
                }
            }
        }
    }
}
