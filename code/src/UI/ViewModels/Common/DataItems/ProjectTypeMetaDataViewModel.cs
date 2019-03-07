﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class ProjectTypeMetaDataViewModel : BasicInfoViewModel
    {
        private MetadataType _metadataType;
        private string _emptyBackendFramework = string.Empty;

        public MetadataType MetadataType
        {
            get => _metadataType;
            set => SetProperty(ref _metadataType, value);
        }

        public IEnumerable<LayoutViewModel> Layout { get; private set; }

        public IEnumerable<FrameworkMetaDataViewModel> Frameworks { get; }

        public override bool HasLayout => Layout.Any();

        public override bool HasFrameworks => Frameworks.Any();

        public ProjectTypeMetaDataViewModel(MetadataInfo metadataInfo, string platform, IEnumerable<FrameworkMetaDataViewModel> frameworks)
        {
            Name = metadataInfo.Name;
            Identity = metadataInfo.Name;
            Title = metadataInfo.DisplayName;
            Summary = metadataInfo.Summary;
            Description = metadataInfo.Description;
            Author = metadataInfo.Author;
            Icon = metadataInfo.Icon;
            Order = metadataInfo.Order;
            MetadataType = metadataInfo.MetadataType;
            Licenses = metadataInfo.LicenseTerms?.Select(l => new LicenseViewModel(l));
            Frameworks = frameworks;
            var fx = frameworks.First().Name;
            var layout = GenComposer.GetLayoutTemplates(Name, fx, _emptyBackendFramework, platform);
            Layout = layout.Select(l => new LayoutViewModel(l, fx, platform));
        }
    }
}
