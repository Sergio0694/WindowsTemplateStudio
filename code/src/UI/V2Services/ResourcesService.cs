﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.UI.V2Services
{
    public class ResourcesService
    {
        private static ResourcesService _instance;
        private Window _mainView;

        public static ResourcesService Instance => _instance ?? (_instance = new ResourcesService());

        private ResourcesService()
        {
        }

        public void Initialize(Window mainView)
        {
            _mainView = mainView;
        }

        public T FindResource<T>(object resourceKey)
            where T : class
        {
            return FindResource(resourceKey) as T;
        }

        public object FindResource(object resourceKey) => _mainView.FindResource(resourceKey);
    }
}