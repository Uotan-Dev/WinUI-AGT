using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Xaml.Navigation;

namespace Toolbox
{
    public sealed partial class Bootloader_Driver : Page
    {
        public MainWindow parent { get; set; }

        public class BootloaderDriverParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is BootloaderDriverParameter parameter)
            {
                parent = parameter.Parent;
            }
        }

        //定义窗口
        public Bootloader_Driver()
        {
            this.InitializeComponent();
        }

        // Bootloader文件选择器
        private async void PickUnlockFileClick(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            PickUnlockFileOutputTextBlock.Text = "";

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                PickUnlockFileOutputTextBlock.Text = "Picked file: " + file.Name;
            }
            else
            {
                PickUnlockFileOutputTextBlock.Text = "Operation cancelled.";
            }
        }

        // Bootloader解锁按钮
        private void UnlockBootloaderClick(object sender, RoutedEventArgs e)
        {
            parent.Checkcon();
            if (parent.ConnInfoText == "Fastboot")
            {
                if (PickUnlockFileOutputTextBlock.Text != "")
                {
                    string file = PickUnlockFileOutputTextBlock.Text;
                    string shell = string.Format("flash unlock {0}", file);
                    string shell2 = "oem unlock-go";
                    ADBHelper.Fastboot(shell);
                    int sf = ADBHelper.Fastboot(shell2).IndexOf("OKAY");
                    if (sf != -1)
                    {
                        parent.ShowDialog("解锁成功！");
                    }
                    else
                    {
                        parent.ShowDialog("解锁失败！");
                    }
                }
                else
                {
                    parent.ShowDialog("请选择解锁文件！");
                }
            }
            else
            {
                parent.ShowDialog("请进入Fastboot模式！");
            }
        }


        private void RelockBootloaderClick(object sender, RoutedEventArgs e)
        {
            parent.Checkcon();
            if (parent.ConnInfoText == "Fastboot")
            {
                string shell = "oem lock-go";
                string shell2 = "flashing lock";
                ADBHelper.Fastboot(shell);
                int sf = ADBHelper.Fastboot(shell2).IndexOf("OKAY");
                if (sf != -1)
                {
                    parent.ShowDialog("回锁成功！");
                }
                else
                {
                    parent.ShowDialog("回锁失败！");
                }
            }
            else
            {
                parent.ShowDialog("请进入Fastboot模式！");
            }
        }

        private async void OemUnlockClick(object sender, RoutedEventArgs e)
        {
            parent.Checkcon();
            if (UnlockShell.SelectedIndex != -1)
            {
                if (parent.ConnInfoText == "Fastboot")
                {
                    bool result = await parent.ShowDialogYesOrNo("该功能仅支持部分品牌设备！\n\r执行后您的设备应当出现确认解锁提示，\n\r若未出现则为您的设备不支持该操作。");
                    if (result == true)
                    {
                        string shell = UnlockShell.Text;
                        ADBHelper.Fastboot(shell);
                        parent.ShowDialog("执行完成，请查看您的设备！");
                    }
                }
                else
                {
                    parent.ShowDialog("请进入Fastboot模式！");
                }
            }
            else
            {
                parent.ShowDialog("请选择解锁命令！");
            }
        }

        private void XiaomiUnlockClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"unlock\xiaomi\batch_unlock.exe");
        }

        private void ViqooUnlockClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"unlock\vivo\launch.exe");
        }
    }
}
