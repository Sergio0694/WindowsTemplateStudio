﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2ViewModels.NewProject;

namespace Microsoft.Templates.UI.V2Services
{
    public static class OrderingService
    {
        private static ObservableCollection<SavedTemplateViewModel> Pages
        {
            get
            {
                return MainViewModel.Instance.UserSelection.Pages;
            }
        }

        public static void Initialize(ListView listView)
        {
            var service = new DragAndDropService<SavedTemplateViewModel>(listView)
            {
                CanDragItem = CanDragItem
            };
            service.ProcessDrop += OnDrop;
        }

        private static bool CanDragItem(SavedTemplateViewModel savedTemplate)
        {
            return savedTemplate != null && savedTemplate.GenGroup == 0;
        }

        private static void OnDrop(object sender, DragAndDropEventArgs<SavedTemplateViewModel> e)
        {
            var items = Pages.Where(p => p.GenGroup == e.ItemData.GenGroup);
            if (items.Count() > 1)
            {
                if (e.OldIndex > -1)
                {
                    Pages.Move(e.OldIndex, e.NewIndex);
                    EventService.Instance.RaiseOnReorderTemplate();
                }
            }
        }
    }
}