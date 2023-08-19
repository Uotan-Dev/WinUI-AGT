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

        //���崰��
        public BootloaderDriver()
        {
            this.InitializeComponent();
        }

        // �ļ�ѡ����
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
            else PickUnlockFileOutputTextBlock.PlaceholderText = "������ȡ��";
        }

        // Bootloader������ť
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
                    if (sf != -1) _ = Parent.ShowDialog("�����ɹ���");
                    else _ = Parent.ShowDialog("����ʧ�ܣ�");
                }
                else _ = Parent.ShowDialog("��ѡ������ļ���");
            }
            else  _ = Parent.ShowDialog("�����Fastbootģʽ��");
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
                if (sf != -1)  _ = Parent.ShowDialog("�����ɹ���");
                else _ = Parent.ShowDialog("����ʧ�ܣ�");
            }
            else _ = Parent.ShowDialog("�����Fastbootģʽ��");
        }

        private async void OemUnlockClick(object sender, RoutedEventArgs e)
        {
            if (UnlockShell.SelectedIndex != -1)
            {
                Parent.CheckconAsync();
                if (Parent.ConnInfoText == "Fastboot")
                {
                    bool result = await Parent.ShowDialogYesOrNo("�ù��ܽ�֧�ֲ���Ʒ���豸��\n\rִ�к������豸Ӧ������ȷ�Ͻ�����ʾ��\n\r��δ������Ϊ�����豸��֧�ָò�����");
                    if (result == true)
                    {
                        string shell = (UnlockShell.SelectedValue as ComboBoxItem)?.Content.ToString();
                        _ = ADBHelper.Fastboot(shell);
                        _ = Parent.ShowDialog("ִ����ɣ���鿴�����豸��");
                    }
                }
                else _ = Parent.ShowDialog("�����Fastbootģʽ��");
            }
            else _ = Parent.ShowDialog("��ѡ��������");
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
            _ = Parent.ShowDialog("��������Ҫ����Ȩ�ޣ�\n��ʹ�ù���Ա������´򿪱�Ӧ�á�");
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
                _ = Parent.ShowDialog("ִ�гɹ���");
            }
            else
                RestartTip();
        }
    }
}
