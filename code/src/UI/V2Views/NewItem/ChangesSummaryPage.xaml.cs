﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Controls;
using Microsoft.Templates.UI.V2ViewModels.NewItem;

namespace Microsoft.Templates.UI.V2Views.NewItem
{
    public partial class ChangesSummaryPage : Page
    {
        public ChangesSummaryPage(NewItemGenerationResult output)
        {
            DataContext = MainViewModel.Instance;
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                MainViewModel.Instance.ChangesSummary.Initialize(output);
            };
        }
    }
}