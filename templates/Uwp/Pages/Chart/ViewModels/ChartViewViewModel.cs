﻿using System;
using System.Collections.ObjectModel;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ChartViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ChartViewViewModel()
        {
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetChartSampleData();
            }
        }
    }
}
