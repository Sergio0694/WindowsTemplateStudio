﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Param_RootNamespace.Helpers;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ImageGalleryViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";

        private readonly INavigationService _navigationService;
        private readonly IConnectedAnimationService _connectedAnimationService;

        public BindableCollection<SampleImage> Source { get; } = new BindableCollection<SampleImage>();

        public ImageGalleryViewViewModel(INavigationService navigationService, IConnectedAnimationService connectedAnimationService)
        {
            _navigationService = navigationService;
            _connectedAnimationService = connectedAnimationService;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // TODO WTS: Replace this with your actual data
            Source.AddRange(SampleDataService.GetGallerySampleData());
        }

        public void OnImageSelected(SampleImage image)
        {
            ImagesNavigationHelper.AddImageId(ImageGalleryViewSelectedIdKey, image.ID);
            _connectedAnimationService.SetListDataItemForNextConnectedAnnimation(image);
            _navigationService.For<ImageGalleryViewDetailViewModel>()
                .WithParam(v => v.ID, image.ID)
                .Navigate();
        }
    }
}
