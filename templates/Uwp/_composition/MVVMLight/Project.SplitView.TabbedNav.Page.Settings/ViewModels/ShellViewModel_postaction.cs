﻿//{**
//This code block adds the logic to handle SettingsItem in NavigationView control from ViewModel.
//**}
//{[{
using Param_RootNamespace.Views;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            //{[{
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(wts.ItemNameViewModel).FullName);
                return;
            }

            //}]}
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            //{[{
            if (e.SourcePageType == typeof(wts.ItemNamePage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            //}]}
        }
    }
}
