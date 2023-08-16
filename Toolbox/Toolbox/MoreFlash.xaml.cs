using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Text;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.IO;

namespace Toolbox
{
    public sealed partial class MoreFlash : Page
    {
        public new MainWindow Parent { get; set; }

        public class MoreFlashParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MoreFlashParameter parameter)
            {
                Parent = parameter.Parent;
            }
        }

        private readonly List<Button> buttons;

        //定义窗口
        public MoreFlash()
        {
            this.InitializeComponent();
            buttons = new List<Button>
            {
                Flash9008Button, FlashAdbSideloadButton
            };
        }

        private void DisableAllButtons() { foreach (var button in buttons) button.IsEnabled = false; }
        private void EnableAllButtons() { foreach (var button in buttons) button.IsEnabled = true; }

        // 实时显示FastBoot输出的方法
        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!string.IsNullOrEmpty(outLine.Data))
            {
                StringBuilder sb = new(FlashShow.Text);
                FlashShow.Text = sb.AppendLine(outLine.Data).ToString();
                FlashShow.SelectionStart = FlashShow.Text.Length;

                var scrollViewer = FindChild<ScrollViewer>(FlashShow);
                scrollViewer.ChangeView(null, scrollViewer.ExtentHeight, null);
            }
        }

        // 在VisualTree中查找指定类型的子元素
        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;
                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        public void FBontime(string fb)
        {
            string cmd = @"lib\fastboot.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true
            };
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        public void Bat(string batpatch, string exepatch)//调用Bat
        {
            string wkdir = String.Format(@"{0}\lib", exepatch);
            Process process = null;
            process = new Process();
            process.StartInfo.FileName = batpatch;
            process.StartInfo.WorkingDirectory = wkdir;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        public void ADB(string fb)//ADB实时输出
        {
            string cmd = @"bin\adb.exe";
            ProcessStartInfo adb = null;
            adb = new ProcessStartInfo(cmd, fb)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process f = Process.Start(adb);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        public void QSaharaServer(string fb)//QSaharaServer实时输出
        {
            string cmd = @"bin\QSaharaServer.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        public void Fhloader(string fb)//Fhloader实时输出
        {
            string cmd = @"bin\fh_loader.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        // 文件选择器
        private async void PickAdbSideloadFileClick(object sender, RoutedEventArgs e)
        {
            PickAdbSideloadFileOutput.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickAdbSideloadFileOutput.Text = "已选择 " + file.Name;
            else PickAdbSideloadFileOutput.Text = "操作已取消";
        }

        private async void PickElfFileClick(object sender, RoutedEventArgs e)
        {
            PickElfFileOutput.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickElfFileOutput.Text = "已选择 " + file.Name;
            else PickElfFileOutput.Text = "操作已取消";
        }

        private async void PickXmlFileClick(object sender, RoutedEventArgs e)
        {
            PickXmlFileOutput.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickXmlFileOutput.Text = "已选择 " + file.Name;
            else PickXmlFileOutput.Text = "操作已取消";
        }

        private void FlashAdbSideloadClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Sideload")
            {
                if (PickAdbSideloadFileOutput.Text != "")
                {
                    DisableAllButtons();
                    FlashShow.Text = "";
                    string shell = String.Format("sideload {0}", PickAdbSideloadFileOutput.Text);
                    ADB(shell);
                    _ = Parent.ShowDialog("执行完成！");
                    EnableAllButtons();
                }
                else _ = Parent.ShowDialog("请选择刷机包！");
            }
            else _ = Parent.ShowDialog("请将设备进入Sideload模式后执行！");
        }

        private void Flash9008Click(object sender, RoutedEventArgs e)
        {
            if (PickElfFileOutput.Text != "" && PickXmlFileOutput.Text != "")
            {
                DisableAllButtons();
                FlashShow.Text = "";
                string storage = "";
                if (UfsToggle.IsChecked == true) storage = "UFS";
                if (EmmcToggle.IsChecked == true) storage = "EMMC";
                string usbdevices = Mindows.Devcon("find usb*");
                if (usbdevices.IndexOf("QDLoader") != -1)
                {
                    string com = Mindows.FindEDLCom(usbdevices);
                    string elf = PickElfFileOutput.Text;
                    string imgdir = Path.GetDirectoryName(elf);
                    string xml = PickXmlFileOutput.Text;
                    string shell = String.Format(@"-p \\.\{0} -s 13:{1}", com, elf);
                    QSaharaServer(shell);
                    shell = String.Format(@"--port=\\.\{0} --search_path={1} --memoryname={2} --noprompt --sendxml={3} --zlpawarehost=1 --reset", com, imgdir, storage, xml);
                    Fhloader(shell);
                }
                else _ = Parent.ShowDialog("请将设备进入9008模式！");
                EnableAllButtons();
            }
            else _ = Parent.ShowDialog("请选择ELF和XML文件！");
        }
    }
}
