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
    public sealed partial class BootloaderDriver : Page
    {
        public new MainWindow Parent { get; set; }

        public class BootloaderDriverParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is BootloaderDriverParameter parameter)
            {
                Parent = parameter.Parent;
            }
        }

        //定义窗口
        public BootloaderDriver()
        {
            this.InitializeComponent();
        }

        // 文件选择器
        private async void PickUnlockFileClick(object sender, RoutedEventArgs e)
        {
            PickUnlockFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickUnlockFileOutputTextBlock.Text = file.Path;
            else PickUnlockFileOutputTextBlock.PlaceholderText = "操作已取消";
        }

        // Bootloader解锁按钮
        private async void UnlockBootloaderClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                if (PickUnlockFileOutputTextBlock.Text != "")
                {
                    string file = PickUnlockFileOutputTextBlock.Text;
                    string shell = string.Format("flash unlock {0}", file);
                    string shell2 = "oem unlock-go";
                    _ = ADBHelper.Fastboot(shell);
                    string sfstring = await ADBHelper.Fastboot(shell2);
                    int sf = sfstring.Contains("OKAY") ? sfstring.IndexOf("OKAY") : -1;
                    if (sf != -1) _ = Parent.ShowDialog("解锁成功！");
                    else _ = Parent.ShowDialog("解锁失败！");
                }
                else _ = Parent.ShowDialog("请选择解锁文件！");
            }
            else  _ = Parent.ShowDialog("请进入Fastboot模式！");
        }


        private async void RelockBootloaderClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                string shell = "oem lock-go";
                string shell2 = "flashing lock";
                _ = ADBHelper.Fastboot(shell);
                string sfstring = await ADBHelper.Fastboot(shell2);
                int sf = sfstring.Contains("OKAY") ? sfstring.IndexOf("OKAY") : -1;
                if (sf != -1)  _ = Parent.ShowDialog("回锁成功！");
                else _ = Parent.ShowDialog("回锁失败！");
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }

        private async void OemUnlockClick(object sender, RoutedEventArgs e)
        {
            if (UnlockShell.SelectedIndex != -1)
            {
                Parent.CheckconAsync();
                if (Parent.ConnInfoText == "Fastboot")
                {
                    bool result = await Parent.ShowDialogYesOrNo("该功能仅支持部分品牌设备！\n\r执行后您的设备应当出现确认解锁提示，\n\r若未出现则为您的设备不支持该操作。");
                    if (result == true)
                    {
                        string shell = (UnlockShell.SelectedValue as ComboBoxItem)?.Content.ToString();
                        _ = ADBHelper.Fastboot(shell);
                        _ = Parent.ShowDialog("执行完成，请查看您的设备！");
                    }
                }
                else _ = Parent.ShowDialog("请进入Fastboot模式！");
            }
            else _ = Parent.ShowDialog("请选择解锁命令！");
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
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void RestartTip()
        {
            _ = Parent.ShowDialog("此命令需要提升权限，\n请使用管理员身份重新打开本应用。");
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
                _ = Parent.ShowDialog("执行成功！");
            }
            else
                RestartTip();
        }
    }
}
