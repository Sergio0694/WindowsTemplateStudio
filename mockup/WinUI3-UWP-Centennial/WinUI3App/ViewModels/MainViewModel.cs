﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Popups;

namespace WinUI3App.ViewModels
{
    public class MainViewModel : ObservableRecipient
    {
        private ICommand _showContentInFolderCommand;
        private ICommand _minimizeCommand;
        private ICommand _maximizeCommand;
        private ICommand _restoreCommand;
        private ICommand _sendToBottomCommand;

        public ICommand ShowContentInFolderCommand => _showContentInFolderCommand ?? (_showContentInFolderCommand = new AsyncRelayCommand(OnShowContentInFolder));

        public ICommand MinimizeCommand => _minimizeCommand ?? (_minimizeCommand = new RelayCommand(OnMinimize));        

        public ICommand MaximizeCommand => _maximizeCommand ?? (_maximizeCommand = new RelayCommand(OnMaximize));

        public ICommand RestoreCommand => _restoreCommand ?? (_restoreCommand = new RelayCommand(OnRestore));

        public ICommand SendToBottomCommand => _sendToBottomCommand ?? (_sendToBottomCommand = new RelayCommand(OnSendToBottom));

        public MainViewModel()
        {
        }        

        private async Task OnShowContentInFolder()
        {
#if CENTENNIAL
            var path = @"C:/";
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                return;
            }
            var sb = new StringBuilder();
            sb.AppendLine("Status: running");
            int depth = 0;
            _list.Clear();
            var sw = Stopwatch.StartNew();
            ListDirectories(path, depth);
            sw.Stop();

            sb.Append(_list);
            sb.AppendLine($"Status: completed in {sw.Elapsed.TotalMilliseconds} ms");
            var dialog = new MessageDialog(sb.ToString());
            await dialog.ShowAsync();   
#else
            await ShowWin32OnlyErrorAsync();
#endif
        }

#if CENTENNIAL
        private StringBuilder _list = new StringBuilder();
        private void ListDirectories(string dir, int depth)
        {
            if (depth < 0)
                return;
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    _list.AppendLine($"{d}");
                    ListDirectories(d, depth - 1);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                _list.AppendLine($"Unauthorized Access: {dir} - {ex.Message}");
            }
            catch (PathTooLongException ex)
            {
                _list.AppendLine($"Path Too Long: {dir} - {ex.Message}");
            }
            catch (System.Exception ex)
            {
                _list.AppendLine($"Exception: {ex.Message}");
            }
        }
#endif

        private async void OnMinimize()
        {
#if CENTENNIAL
            IntPtr hwnd = (App.Current as App).WindowHandle;
            PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_MINIMIZE);
#else
            await ShowWin32OnlyErrorAsync();
#endif
        }

        private async void OnMaximize()
        {
#if CENTENNIAL
            IntPtr hwnd = (App.Current as App).WindowHandle;
            PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
#else
            await ShowWin32OnlyErrorAsync();
#endif
        }

        private async void OnRestore()
        {
#if CENTENNIAL
            IntPtr hwnd = (App.Current as App).WindowHandle;
            PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_RESTORE);
#else
            await ShowWin32OnlyErrorAsync();
#endif
        }

        private async void OnSendToBottom()
        {
#if CENTENNIAL
            IntPtr hwnd = (App.Current as App).WindowHandle;
            PInvoke.User32.SetWindowPos(hwnd, PInvoke.User32.SpecialWindowHandles.HWND_BOTTOM, 0, 0, 0, 0, PInvoke.User32.SetWindowPosFlags.SWP_NOMOVE | PInvoke.User32.SetWindowPosFlags.SWP_NOSIZE | PInvoke.User32.SetWindowPosFlags.SWP_NOACTIVATE);
#else
            await ShowWin32OnlyErrorAsync();
#endif
        }

        private async Task ShowWin32OnlyErrorAsync()
        {
            var dialog = new MessageDialog("This feature is exclusive for Win32 Apps") { Title = "Packaging error" };
            await dialog.ShowAsync();
        }
    }
}