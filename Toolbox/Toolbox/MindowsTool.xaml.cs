using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System.Security.Principal;
using System.IO;
using System.Threading.Tasks;

namespace Toolbox
{
    public sealed partial class MindowsTool : Page
    {
        public new MainWindow Parent { get; set; }

        public class MindowsToolParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MindowsToolParameter parameter)
            {
                Parent = parameter.Parent;
            }
        }

        //定义窗口
        public MindowsTool()
        {
            this.InitializeComponent();
        }

        // 文件选择器
        /*private async void PickUnlockFileClick(object sender, RoutedEventArgs e)
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
        }*/

        // 打开一键安装工具
        private async void OpenMindowsClick(object sender, RoutedEventArgs e)
        {
            Setdevice();
            Parent.CheckconAsync();
            Mindows.Disdevice();
            if (Parent.ConnInfoText == "Fastboot")
            {
                string mindowspath1 = String.Format(@"{0}\data\mindows\img\uefi.img", exepath);
                string mindowspath2 = String.Format(@"{0}\data\mindows\img\automass.img", exepath);
                string mindowspath3 = String.Format(@"{0}\data\mindows\img\recovery.img", exepath);
                if (File.Exists(mindowspath1) && File.Exists(mindowspath2) && File.Exists(mindowspath3))
                {
                    if (Global.device != "")
                    {
                        bool result = await Parent.ShowDialogYesOrNo("请确保设备已格式化Data分区！");
                        if (result == true)
                        {
                            bool resultdl = await Parent.ShowDialogYesOrNo("请确认您是否已下载适用的\r\nWindows镜像");
                            if (resultdl == true)
                            {
                                _ = Parent.ShowDialog("欢迎使用Mindows一键安装！此工具是为手机刷入Windows打造的刷机向导，它将引导你完成安卓+Windows双系统的部署。希望它能帮助你顺利完成刷机！在开始之前，请务必认真阅读以下内容：\r\n\r\n-此工具完全免费， 禁止任何人以任何形式商用，包括但不限于贩卖，付费远程刷机等。禁止各种形式的二改套壳打包。\r\n-RenegadeProject项目完全免费，禁止任何人以任何形式商用，包括但不限于付费远程刷机，提供有偿刷机指导，贩卖相关刷机包、刷机工具、刷机教程等。\r\n-如发现违反上述规定的行为，请加QQ1330250642举报！感谢您的支持。\r\n\r\n-刷机完成后设备中将同时存在安卓和Windows系统。这会占用一部分安卓的空间。\r\n-刷机会清空设备内所有数据，请务必提前将个人数据备份到设备之外的地方。\r\n-使用此工具需要先解锁BL锁。请确保你的设备BL锁已解锁。\r\n-刷Windows需要修改分区表，如果你人为修改过分区表，建议先恢复官方分区表再继续。\r\n\r\n-刷机可能导致设备系统损坏无法开机。\r\n-刷机可能会影响系统OTA更新功能，刷机后请勿使用系统更新。\r\n-刷机自愿，工具作者不会恶意损坏你的设备，也不为任何可能发生的意外负责。");
                                bool resultur = await Parent.ShowDialogYesOrNo("警告！\r\n请确保你有充足的时间进行刷机，遇到错误请保留现场并联系开发者\r\n安装Windows会涉及修改分区，此操作风险较大容易变砖，\r\n且修改分区后将无法使用MiFlash在Fastboot模式刷机！\r\n\r\n请确认您已阅读以上内容并同意自愿刷机！");
                                if (resultur == true)
                                {
                                    MindowsInstall form2 = new();
                                    form2.Activate();
                                }
                            }
                            else Mindows.OpenDefaultBrowserUrl("https://www.123pan.com/s/8eP9-BkTGA");
                        }
                    }
                    else _ = Parent.ShowDialog("请选择机型！");
                }
                else _ = Parent.ShowDialog("请先下载资源！");
            }
            else _ = Parent.ShowDialog("请进入Fastboot模式！");
        }

        //机型资源下载链接储存
        readonly string mix2simglink = "";
        readonly string mix2sdrivelink = "";
        readonly string mi8imglink = "";
        readonly string mi8drivelink = "";
        readonly string mix3imglink = "";
        readonly string mix3drivelink = "";
        readonly string pad5imglink = "";
        readonly string pad5drivelink1 = "";
        readonly string pad5drivelink2 = "";
        readonly string mi9imglink = "";
        readonly string mi9drivelink1 = "";
        readonly string mi9drivelink2 = "";
        readonly string mi9drivelink3 = "";
        readonly string mi6imglink = "";
        readonly string mi6drivelink = "";
        readonly string k20pdrivelink1 = "";
        readonly string k20pdrivelink2 = "";
        readonly string k20pdrivelink3 = "";
        readonly string k20pimglink = "";

        //获取工具运行路径
        readonly string exepath = System.IO.Directory.GetCurrentDirectory();

        public void Setdevice()
        {
            string SelectedDevice = (ChooseModel.SelectedValue as ComboBoxItem)?.Content.ToString();
            if (SelectedDevice == "小米MIX2S")
            {
                Global.device = "MIX2S";
                Global.drivelink1 = mix2sdrivelink;
                Global.imglink = mix2simglink;
            }
            if (SelectedDevice == "小米8")
            {
                Global.device = "MI8";
                Global.drivelink1 = mi8drivelink;
                Global.imglink = mi8imglink;
            }
            if (SelectedDevice == "小米MIX3")
            {
                Global.device = "MIX3";
                Global.drivelink1 = mix3drivelink;
                Global.imglink = mix3imglink;
            }
            if (SelectedDevice == "小米平板5")
            {
                Global.device = "Pad5";
                Global.drivelink1 = pad5drivelink1;
                Global.drivelink2 = pad5drivelink2;
                Global.imglink = pad5imglink;
            }
            if (SelectedDevice == "小米9")
            {
                Global.device = "MI9";
                Global.drivelink1 = mi9drivelink1;
                Global.drivelink2 = mi9drivelink2;
                Global.drivelink3 = mi9drivelink3;
                Global.imglink = mi9imglink;
            }
            if (SelectedDevice == "Redmi K20 Pro")
            {
                Global.device = "K20Pro";
                Global.drivelink1 = k20pdrivelink1;
                Global.drivelink2 = k20pdrivelink2;
                Global.drivelink3 = k20pdrivelink3;
                Global.imglink = k20pimglink;
            }
            if (SelectedDevice == "小米6")
            {
                Global.device = "MI6";
                Global.drivelink1 = mi6drivelink;
                Global.imglink = mi6imglink;
            }
            if (SelectedDevice == "小米MIX2")
            {
                Global.device = "MIX2";
            }
        }

        // 下载资源
        private async void DownloadResource(object sender, RoutedEventArgs e)
        {
            Setdevice();
            Mindows.Disdevice();
            string mindowspath = String.Format(@"{0}\data\mindows", exepath);
            if (Directory.Exists(mindowspath))
            {
                if (Global.device != "")
                {
                    bool stardownload = true;
                    try
                    {
                        File.Delete(@"data\mindows\driver.7z.001");
                        File.Delete(@"data\mindows\driver.7z.002");
                        File.Delete(@"data\mindows\driver.7z.003");
                        File.Delete(@"data\mindows\img.7z.001");
                    }
                    catch
                    {
                        _ = Parent.ShowDialog("删除临时文件失败，请重启应用后再尝试下载！");
                        stardownload = false;
                    }
                    if (stardownload)
                    {
                        mindowspath = String.Format(@"{0}\data\mindows\img", exepath);
                        if (Global.drivelink1 != "" || Global.imglink != "")
                        {
                            if (!Directory.Exists(mindowspath))
                            {
                                if (Global.warn)
                                {
                                    DeviceWarn warning = new DeviceWarn();
                                    warning.Activate();
                                }
                                else
                                {
                                    DownloadResource downlaoder = new DownloadResource();
                                    downlaoder.Activate();
                                }
                            }
                            else
                            {
                                bool result = await Parent.ShowDialogYesOrNo("您已下载资源！是否重新下载？");
                                if (result == true)
                                {
                                    Mindows.DeleteFolder(@"data\mindows\driver");
                                    Mindows.DeleteFolder(@"data\mindows\img");
                                    if (Global.warn)
                                    {
                                        DeviceWarn warning = new DeviceWarn();
                                        warning.Activate();
                                    }
                                    else
                                    {
                                        DownloadResource downlaoder = new DownloadResource();
                                        downlaoder.Activate();
                                    }
                                }
                            }
                        }
                        else _ = Parent.ShowDialog("未能获取下载链接，请确认网络正常并重启程序！");
                    }
                }
                else _ = Parent.ShowDialog("请选择机型！");
            }
            else _ = Parent.ShowDialog("文件夹丢失！");
        }

        // 注册表相关
        private async void SkipWebOobe(object sender, RoutedEventArgs e)
        {
            Setdevice();
            if (Global.device != "")
            {
                bool result = await Parent.ShowDialogYesOrNo("请先将设备进入大容量模式！");
                if (result == true)
                {
                    Global.moreability = "noweboobe";
                    MindowsWidget form = new();
                    form.Activate();
                }
            }
            else
            {
                _ = Parent.ShowDialog("未选择机型！");
            }
        }
    }
}
