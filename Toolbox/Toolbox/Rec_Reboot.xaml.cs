using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Toolbox
{
    public sealed partial class Rec_Reboot : Page
    {
        public new MainWindow Parent { get; set; }

        public class RecRebootParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is RecRebootParameter parameter)
            {
                Parent = parameter.Parent;
            }
        }

        private readonly List<Button> buttons;

        public Rec_Reboot()
        {
            this.InitializeComponent();
            buttons = new List<Button>
            {
                PickRecFile, 
                FlashRec, FlashRecA, FlashRecB, 
                BootImg, FlashBootA, FlashBootB, 
                DisableOffRec, FlashMagisk
            };
        }

        // Recovery文件选择器
        private async void PickRecFileClick(object sender, RoutedEventArgs e)
        {
            PickRecFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickRecFileOutputTextBlock.Text = "已选择 " + file.Name;
            else PickRecFileOutputTextBlock.Text = "操作已取消";
        }

        private void DisableAllButtons() { foreach (var button in buttons) button.IsEnabled = false; }
        private void EnableAllButtons() { foreach (var button in buttons) button.IsEnabled = true; }

        // FlashRecClick
        private async void FlashRecCore(string mode)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                if (PickRecFileOutputTextBlock.Text != "")
                {
                    DisableAllButtons();
                    string file = PickRecFileOutputTextBlock.Text;
                    string shell_command = "flash " + mode + " {0}";
                    string shell = string.Format(shell_command, file);
                    string sfstring = await ADBHelper.Fastboot(shell);
                    int sf = sfstring.Contains("FAILED") ? sfstring.IndexOf("FAILED") : -1;
                    int sf1 = sfstring.Contains("error") ? sfstring.IndexOf("error") : -1;
                    if (sf == -1 && sf1 == -1)
                    {
                        bool result = await Parent.ShowDialogYesOrNo("刷入成功！是否重启到Recovery？");
                        if (result == true)
                        {
                            string output = await ADBHelper.Fastboot("oem reboot-recovery");
                            if (output.IndexOf("unknown command") != -1)
                            {
                                _ = ADBHelper.Fastboot("reboot recovery");
                            }
                        }
                    }
                    else _ = Parent.ShowDialog("刷入失败！");
                    EnableAllButtons();
                }
                else _ = Parent.ShowDialog("请选择Recovery文件！");
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }

        private async void BootImgClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                if (PickRecFileOutputTextBlock.Text != "")
                {
                    DisableAllButtons();
                    string file = PickRecFileOutputTextBlock.Text;
                    string shell = string.Format("boot {0}", file);
                    string sfstring = await ADBHelper.Fastboot(shell);
                    int sf = sfstring.IndexOf("Finished");
                    if (sf != -1) _ = Parent.ShowDialog("启动成功！");
                    else _ = Parent.ShowDialog("启动失败！");
                    EnableAllButtons();
                }
                else _ = Parent.ShowDialog("请选择镜像文件！");
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }

        private void FlashRecClick(object sender, RoutedEventArgs e) { FlashRecCore("recovery"); }
        private void FlashRecAClick(object sender, RoutedEventArgs e) { FlashRecCore("recovery_a"); }
        private void FlashRecBClick(object sender, RoutedEventArgs e) { FlashRecCore("recovery_b"); }
        private void FlashBootAClick(object sender, RoutedEventArgs e){ FlashRecCore("boot_a"); }
        private void FlashBootBClick(object sender, RoutedEventArgs e) { FlashRecCore("boot_b"); }

        private void DisableOffRecClick(object sender, RoutedEventArgs e) {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Recovery")
            {
                DisableAllButtons();
                string shell = "push lib/DisableAutoRecovery.zip /tmp/";
                string shell2 = "shell twrp install /tmp/DisableAutoRecovery.zip";
                _ = ADBHelper.ADB(shell);
                _ = ADBHelper.ADB(shell2);
                _ = Parent.ShowDialog("执行完成！");
                EnableAllButtons();
            }
            else _ = Parent.ShowDialog("请将设备进入Recovery模式后执行！");
        }
        private void FlashMagiskClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Recovery")
            {
                DisableAllButtons();
                string shell = "push lib/Magisk-v25.2.zip /tmp/";
                string shell2 = "shell twrp install /tmp/Magisk-v25.2.zip";
                _ = ADBHelper.ADB(shell);
                _ = ADBHelper.ADB(shell2);
                _ = Parent.ShowDialog("执行完成！");
                EnableAllButtons();
            }
            else _ = Parent.ShowDialog("请将设备进入Recovery模式后执行！");
        }

        private async void FastbootRebootClick(object sender, RoutedEventArgs e)
        {
            if (FastbootRebootMode.SelectedIndex != -1)
            {
                Parent.CheckconAsync();
                if (Parent.ConnInfoText == "Fastboot")
                {
                    string mode = (FastbootRebootMode.SelectedValue as ComboBoxItem)?.Content.ToString();
                    if (mode == "Recovery")
                    {
                        string output = await ADBHelper.Fastboot("oem reboot-recovery");
                        if (output.IndexOf("unknown command") != -1) _ = ADBHelper.Fastboot("reboot recovery");
                    }
                    else if (mode == "系统") _ = ADBHelper.Fastboot("reboot");
                    else if (mode == "9008") _ = ADBHelper.Fastboot("oem edl");
                    else if (mode == "Fastbootd") _ = ADBHelper.Fastboot("reboot-fastboot");
                    else if (mode == "Bootloader") _ = ADBHelper.Fastboot("reboot-bootloader");

                    _ = Parent.ShowDialog("执行完成，请查看您的设备！");
                }
                else _ = Parent.ShowDialog("请进入Fastboot模式！");
            }
            else _ = Parent.ShowDialog("请选择重启选项！");
        }

        private void AdbRebootClick(object sender, RoutedEventArgs e)
        {
            if (AdbRebootMode.SelectedIndex != -1)
            {
                Parent.CheckconAsync();
                string mode = (AdbRebootMode.SelectedValue as ComboBoxItem)?.Content.ToString();
                if (mode == "Recovery") _ = ADBHelper.ADB("reboot recovery");
                else if (mode == "系统") _ = ADBHelper.ADB("reboot");
                else if (mode == "9008") _ = ADBHelper.ADB("reboot edl");
                else if (mode == "Fastbootd") _ = ADBHelper.ADB("reboot fastboot");
                else if (mode == "Bootloader") _ = ADBHelper.ADB("reboot bootloader");
                else if (mode == "Sideload") _ = ADBHelper.ADB("reboot sideload");
                else if (mode == "TWRP重启至Sideload") _ = ADBHelper.ADB("shell twrp sideload");

                _ = Parent.ShowDialog("执行完成，请查看您的设备！");
            }
            else _ = Parent.ShowDialog("请选择重启选项！");
        }
    }
}
