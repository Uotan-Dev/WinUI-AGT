using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System.Security.Principal;


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
            PickUnlockFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickUnlockFileOutputTextBlock.Text = "已选择 " + file.Name;
            else PickUnlockFileOutputTextBlock.Text = "操作已取消";
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
            if (UnlockShell.SelectedIndex != -1)
            {
                parent.Checkcon();
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

        static bool IsUserAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RestartTip()
        {
            parent.ShowDialog("此命令需要提升权限，\n请使用管理员身份重新打开本应用。");
        }

        private void InstallAdbClick(object sender, RoutedEventArgs e)
        {
            if (IsUserAdministrator())
                System.Diagnostics.Process.Start(@"lib\adb_driver.exe");
            else
                RestartTip();
        }
        private void EdlDrvClick(object sender, RoutedEventArgs e)
        {
            if (IsUserAdministrator())
                System.Diagnostics.Process.Start(@"lib\Qualcomm_Usb_Driver.exe");
            else
                RestartTip();
        }
        private void FixUsbClick(object sender, RoutedEventArgs e)
        {
            if (IsUserAdministrator())
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\usbflags\18D1D00D0100", true);
                key.SetValue("osvc", new byte[] { 0x00, 0x00 }, RegistryValueKind.Binary);
                key.SetValue("SkipContainerIdQuery", new byte[] { 0x01, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
                key.SetValue("SkipBOSDescriptorQuery", new byte[] { 0x01, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
                key.Close();
                parent.ShowDialog("执行成功！");
            }
            else
                RestartTip();
        }
    }
}
