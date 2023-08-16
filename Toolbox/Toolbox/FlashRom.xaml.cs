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

namespace Toolbox
{
    public sealed partial class FlashRom : Page
    {
        public new MainWindow Parent { get; set; }

        public class FlashRomParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is FlashRomParameter parameter)
            {
                Parent = parameter.Parent;
            }
        }

        private readonly List<Button> buttons;

        //定义窗口
        public FlashRom()
        {
            this.InitializeComponent();
            buttons = new List<Button>
            {
                PickFastbootFile, PickFastbootdFile, PickMiScript,
                SetAButton, SetBButton, FlashRomButton
            };
        }

        private void DisableAllButtons() { foreach (var button in buttons) button.IsEnabled = false; }
        private void EnableAllButtons() { foreach (var button in buttons) button.IsEnabled = true; }

        readonly string exepath = System.IO.Directory.GetCurrentDirectory();//获取工具运行路径

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

        // 文件选择器
        private async void PickFastbootFileClick(object sender, RoutedEventArgs e)
        {
            PickFastbootFileOutput.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickFastbootFileOutput.Text = "已选择 " + file.Name;
            else PickFastbootFileOutput.Text = "操作已取消";
        }

        private async void PickMiScriptClick(object sender, RoutedEventArgs e)
        {
            PickMiScriptOutput.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickMiScriptOutput.Text = "已选择 " + file.Name;
            else PickMiScriptOutput.Text = "操作已取消";
        }

        private async void PickFastbootdFileClick(object sender, RoutedEventArgs e)
        {
            PickFastbootdFileOutput.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickFastbootdFileOutput.Text = "已选择 " + file.Name;
            else PickFastbootdFileOutput.Text = "操作已取消";
        }

        private void SetAClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                FBontime("set_active a");
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }

        private void SetBClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                FBontime("set_active b");
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }

        private async void FlashRomClick(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                if (FlashBatToggle.IsOn)
                {
                    if (PickMiScriptOutput.Text != "")
                    {
                        DisableAllButtons();
                        Bat(PickMiScriptOutput.Text, exepath);
                        EnableAllButtons();
                    }
                    else _ = Parent.ShowDialog("请选择刷机文件！");
                }
                else
                {
                    if (PickFastbootdFileOutput.Text != "" || PickFastbootFileOutput.Text != "")
                    {
                        DisableAllButtons();
                        string fbtxt = PickFastbootFileOutput.Text;
                        string fbdtxt = PickFastbootdFileOutput.Text;
                        string imgpath;
                        if (PickFastbootFileOutput.Text != "")
                        {
                            imgpath = string.Concat(fbtxt.AsSpan(0, fbtxt.LastIndexOf(@"\")), @"\images");
                            string fbparts = Mindows.Readtxt(fbtxt);
                            char[] charSeparators = new char[] { '\n' };
                            string[] fbflashparts = fbparts.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                            string flashpart;
                            for (int i = 0; i < fbflashparts.Length; i++)
                            {
                                if (fbflashparts[i].IndexOf("\r") != -1)
                                {
                                    flashpart = fbflashparts[i][..fbflashparts[i].LastIndexOf("\r")];
                                }
                                else
                                {
                                    flashpart = fbflashparts[i];
                                }
                                string shell = string.Format(@"flash {0} {1}\{2}.img", flashpart, imgpath, flashpart);
                                FBontime(shell);
                            }
                        }
                        if (PickFastbootdFileOutput.Text != "")
                        {
                            FBontime("reboot fastboot");
                            imgpath = string.Concat(fbdtxt.AsSpan(0, fbdtxt.LastIndexOf(@"\")), @"\images");
                            string fbdparts = Mindows.Readtxt(fbdtxt);
                            char[] charSeparators = new char[] { '\n' };
                            string[] fbdflashparts = fbdparts.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                            string flashpart;
                            for (int i = 0; i < fbdflashparts.Length; i++)
                            {
                                if (fbdflashparts[i].IndexOf("\r") != -1)
                                {
                                    flashpart = fbdflashparts[i][..fbdflashparts[i].LastIndexOf("\r")];
                                }
                                else
                                {
                                    flashpart = fbdflashparts[i];
                                }
                                string shell = string.Format(@"flash {0} {1}\{2}.img", flashpart, imgpath, flashpart);
                                FBontime(shell);
                            }
                        }
                        if (FlashBatToggle.IsOn == true)
                        {
                            FBontime("erase metadata");
                            FBontime("erase userdata");
                        }
                        bool result = await Parent.ShowDialogYesOrNo("ROM刷入完成！是否重启到系统？");
                        if (result == true)
                        {
                            FBontime("reboot");
                        }
                        EnableAllButtons();
                    }
                    else _ = Parent.ShowDialog("请选择刷机文件！");
                }
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }
    }
}
