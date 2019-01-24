﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.Extensions
{
    public class TextBlockExtensions
    {
        public static readonly DependencyProperty SequentialFlowStepProperty = DependencyProperty.RegisterAttached(
          "SequentialFlowStep",
          typeof(StepData),
          typeof(TextBlockExtensions),
          new PropertyMetadata(null, OnSequentialFlowStepChanged));

        public static readonly DependencyProperty SequentialFlowStepIndexProperty = DependencyProperty.RegisterAttached(
          "SequentialFlowStepIndex",
          typeof(int),
          typeof(TextBlockExtensions),
          new PropertyMetadata(0, OnSequentialFlowStepChanged));

        public static readonly DependencyProperty SequentialFlowStepCompletedProperty = DependencyProperty.RegisterAttached(
          "SequentialFlowStepCompleted",
          typeof(bool),
          typeof(TextBlockExtensions),
          new PropertyMetadata(false, OnSequentialFlowStepChanged));

        public static void SetSequentialFlowStep(UIElement element, StepData value)
        {
            element.SetValue(SequentialFlowStepProperty, value);
        }

        public static StepData GetSequentialFlowStep(UIElement element)
        {
            return (StepData)element.GetValue(SequentialFlowStepProperty);
        }

        public static void SetSequentialFlowStepIndex(UIElement element, int value)
        {
            element.SetValue(SequentialFlowStepIndexProperty, value);
        }

        public static int GetSequentialFlowStepIndex(UIElement element)
        {
            return (int)element.GetValue(SequentialFlowStepIndexProperty);
        }

        public static void SetSequentialFlowStepCompleted(UIElement element, bool value)
        {
            element.SetValue(SequentialFlowStepCompletedProperty, value);
        }

        public static bool GetSequentialFlowStepCompleted(UIElement element)
        {
            return (bool)element.GetValue(SequentialFlowStepCompletedProperty);
        }

        private static void OnSequentialFlowStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = d as TextBlock;
            var step = GetSequentialFlowStep(textBlock);
            var completed = GetSequentialFlowStepCompleted(textBlock);
            textBlock.Inlines.Clear();
            if (completed)
            {
                textBlock.Inlines.Add(new Run()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Text = char.ConvertFromUtf32(0xE001).ToString(),
                    BaselineAlignment = BaselineAlignment.Center,
                });
                textBlock.Inlines.Add($" {step.Title}");
            }
            else
            {
                textBlock.Inlines.Add($"{step.Index + 1}.  {step.Title}");
            }
        }
    }
}
