using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Toolbox
{
    public sealed partial class Rec_Reboot : Page
    {
        public MainWindow parent { get; set; }

        public class RecRebootParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is RecRebootParameter parameter)
            {
                parent = parameter.Parent;
            }
        }

        private List<Button> buttons;

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
            parent.Checkcon();
            if (parent.ConnInfoText == "Fastboot")
            {
                if (PickRecFileOutputTextBlock.Text != "")
                {
                    DisableAllButtons();
                    string file = PickRecFileOutputTextBlock.Text;
                    string shell_command = "flash " + mode + " {0}";
                    string shell = string.Format(shell_command, file);
                    int sf = ADBHelper.Fastboot(shell).IndexOf("FAILED");
                    int sf1 = ADBHelper.Fastboot(shell).IndexOf("error");
                    if (sf == -1 && sf1 == -1)
                    {
                        bool result = await parent.ShowDialogYesOrNo("刷入成功！是否重启到Recovery？");
                        if (result == true)
                        {
                            string output = ADBHelper.Fastboot("oem reboot-recovery");
                            if (output.IndexOf("unknown command") != -1)
                            {
                                ADBHelper.Fastboot("reboot recovery");
                            }
                        }
                    }
                    else parent.ShowDialog("刷入失败！");
                    EnableAllButtons();
                }
                else parent.ShowDialog("请选择Recovery文件！");
            }
            else parent.ShowDialog("请进入Fastboot模式！");
        }

        private void BootImgClick(object sender, RoutedEventArgs e)
        {
            parent.Checkcon();
            if (parent.ConnInfoText == "Fastboot")
            {
                if (PickRecFileOutputTextBlock.Text != "")
                {
                    DisableAllButtons();
                    string file = PickRecFileOutputTextBlock.Text;
                    string shell = string.Format("boot {0}", file);
                    int sf = ADBHelper.Fastboot(shell).IndexOf("Finished");
                    if (sf != -1) parent.ShowDialog("启动成功！");
                    else parent.ShowDialog("启动失败！");
                    EnableAllButtons();
                }
                else parent.ShowDialog("请选择镜像文件！");
            }
            else parent.ShowDialog("请进入Fastboot模式！");
        }

        private void FlashRecClick(object sender, RoutedEventArgs e) { FlashRecCore("recovery"); }
        private void FlashRecAClick(object sender, RoutedEventArgs e) { FlashRecCore("recovery_a"); }
        private void FlashRecBClick(object sender, RoutedEventArgs e) { FlashRecCore("recovery_b"); }
        private void FlashBootAClick(object sender, RoutedEventArgs e){ FlashRecCore("boot_a"); }
        private void FlashBootBClick(object sender, RoutedEventArgs e) { FlashRecCore("boot_b"); }

        private void DisableOffRecClick(object sender, RoutedEventArgs e) {
            parent.Checkcon();
            if (parent.ConnInfoText == "Recovery")
            {
                DisableAllButtons();
                string shell = "push lib/DisableAutoRecovery.zip /tmp/";
                string shell2 = "shell twrp install /tmp/DisableAutoRecovery.zip";
                ADBHelper.ADB(shell);
                ADBHelper.ADB(shell2);
                parent.ShowDialog("执行完成！");
                EnableAllButtons();
            }
            else parent.ShowDialog("请将设备进入Recovery模式后执行！");
        }
        private void FlashMagiskClick(object sender, RoutedEventArgs e)
        {
            parent.Checkcon();
            if (parent.ConnInfoText == "Recovery")
            {
                DisableAllButtons();
                string shell = "push lib/Magisk-v25.2.zip /tmp/";
                string shell2 = "shell twrp install /tmp/Magisk-v25.2.zip";
                ADBHelper.ADB(shell);
                ADBHelper.ADB(shell2);
                parent.ShowDialog("执行完成！");
                EnableAllButtons();
            }
            else parent.ShowDialog("请将设备进入Recovery模式后执行！");
        }

        private void FastbootRebootClick(object sender, RoutedEventArgs e)
        {
            if (FastbootRebootMode.SelectedIndex != -1)
            {
                parent.Checkcon();
                if (parent.ConnInfoText == "Fastboot")
                {
                    string mode = FastbootRebootMode.Text;
                    if (mode == "Recovery")
                    {
                        string output = ADBHelper.Fastboot("oem reboot-recovery");
                        if (output.IndexOf("unknown command") != -1) ADBHelper.Fastboot("reboot recovery");
                    }
                    else if (mode == "系统") ADBHelper.Fastboot("reboot");
                    else if (mode == "9008") ADBHelper.Fastboot("oem edl");
                    else if (mode == "Fastbootd") ADBHelper.Fastboot("reboot-fastboot");
                    else if (mode == "Bootloader") ADBHelper.Fastboot("reboot-bootloader");

                    parent.ShowDialog("执行完成，请查看您的设备！");
                }
                else parent.ShowDialog("请进入Fastboot模式！");
            }
            else parent.ShowDialog("请选择重启选项！");
        }

        private void AdbRebootClick(object sender, RoutedEventArgs e)
        {
            if (AdbRebootMode.SelectedIndex != -1)
            {
                parent.Checkcon();
                if (parent.ConnInfoText == "Fastboot")
                {
                    string mode = AdbRebootMode.Text;
                    if (mode == "Recovery") ADBHelper.ADB("reboot recovery");
                    else if (mode == "系统") ADBHelper.ADB("reboot");
                    else if (mode == "9008") ADBHelper.ADB("reboot edl");
                    else if (mode == "Fastbootd") ADBHelper.ADB("reboot fastboot");
                    else if (mode == "Bootloader") ADBHelper.ADB("reboot bootloader");
                    else if (mode == "Sideload") ADBHelper.ADB("reboot sideload");
                    else if (mode == "TWRP重启至Sideload") ADBHelper.ADB("shell twrp sideload");

                    parent.ShowDialog("执行完成，请查看您的设备！");
                }
                else parent.ShowDialog("请进入Fastboot模式！");
            }
            else parent.ShowDialog("请选择重启选项！");
        }
    }
}
