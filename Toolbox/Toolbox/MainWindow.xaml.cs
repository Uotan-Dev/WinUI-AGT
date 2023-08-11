using Microsoft.UI.Windowing;
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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Composition.SystemBackdrops;
using Windows.UI.Core;
using System.Threading;
using Windows.Foundation.Metadata;
using static Toolbox.Bootloader_Driver;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Toolbox
{
    public sealed partial class MainWindow : Window
    {
        // 定义主窗口
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "Android 极客工具箱";
            SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();

            // 创建一个临时对象来传递母窗口
            var parent = new BootloaderDriverParameter { Parent = this };
            ContentFrame.Navigate(typeof(Bootloader_Driver), parent);
        }
        public string GetAppTitleFromSystem()
        {
            return Windows.ApplicationModel.Package.Current.DisplayName;
        }

        // 消息弹窗 = MessageBox
        public async void ShowDialog(string content)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.PageContainer.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "提示",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary,
                Content = new ContentDialogContent(content)
            };

            _ = await dialog.ShowAsync();
        }
        public void ShowDialogClick(object sender, RoutedEventArgs e)
        {
            ShowDialog("Congratulations!");
        }

        
        // 搜索框后端
        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }
        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string txt = args.QueryText;  //输入的文本
            if (args.ChosenSuggestion != null)
            {
                //从提示框中选择某一项时触发
            }
            else
            {
                //用户在输入时敲回车或者点击右边按钮确认输入时触发
            }
        }


        // 检查连接函数 - 全局
        public void Checkcon()
        {
            if (ADBHelper.Fastboot("devices") != "")
            {
                conninfo.Text = "Fastboot";
            }
            else
            {
                conninfo.Text = "未连接";
            }
            int adbcheck = ADBHelper.ADB("devices").IndexOf("recovery");
            if (adbcheck != -1)
            {
                conninfo.Text = "Recovery";
            }
            int adbcheck2 = ADBHelper.ADB("devices").IndexOf("sideload");
            if (adbcheck2 != -1)
            {
                conninfo.Text = "Sideload";
            }
            int adbcheck3 = ADBHelper.ADB("devices").IndexOf("	device");
            if (adbcheck3 != -1)
            {
                conninfo.Text = "系统";
            }
            int check9008 = Mindows.Devcon("find usb*").IndexOf("QDLoader");
            if (check9008 != -1)
            {
                conninfo.Text = "9008";
            }
            int check901d = Mindows.Devcon("find usb*").IndexOf("901D (");
            if (check901d != -1)
            {
                conninfo.Text = "901D";
            }
            int check900e = Mindows.Devcon("find usb*").IndexOf("900E");
            if (check900e != -1)
            {
                conninfo.Text = "900E";
            }
            int check9091 = Mindows.Devcon("find usb*").IndexOf("9091 (");
            if (check9091 != -1)
            {
                conninfo.Text = "9091";
            }
            if (conninfo.Text == "Fastboot")
            {
                int unlocked = ADBHelper.Fastboot("getvar unlocked").IndexOf("yes");
                if (unlocked != -1)
                {
                    BLinfo.Text = "已解锁";
                }
                int locked = ADBHelper.Fastboot("getvar unlocked").IndexOf("no");
                if (locked != -1)
                {
                    BLinfo.Text = "未解锁";
                    //Dialog_Display("您的设备未解锁BootLoader！\n\r大部分功能将无法使用！");
                    ShowDialog("您的设备未解锁BootLoader！\n\r大部分功能将无法使用！");
                }
                string productinfos = ADBHelper.Fastboot("getvar product");
                string product = Mindows.GetProductID(productinfos);
                if (product != null)
                {
                    productinfo.Text = product;
                }
                string active = ADBHelper.Fastboot("getvar current-slot");
                if (active.IndexOf("current-slot: a") != -1)
                {
                    VABinfo.Text = "A槽位";
                }
                else if (active.IndexOf("current-slot: b") != -1)
                {
                    VABinfo.Text = "B槽位";
                }
                else if (active.IndexOf("FAILED") != -1)
                {
                    VABinfo.Text = "A-Only设备";
                }
            }
            else
            {
                BLinfo.Text = "未知";
                VABinfo.Text = "未知";
                productinfo.Text = "未知";
            }
        }
        public string ConnInfoText
        {
            get { return conninfo.Text; }
            set { conninfo.Text = value; }
        }

        // 检查链接按钮
        private void CheckconnClick(object sender, RoutedEventArgs e)
        {
            Checkcon();
        }

        // 侧边导航点击切换
        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            if ((string)selectedItem.Tag == "Bootloader_Driver")
            {
                var parent = new BootloaderDriverParameter { Parent = this };
                ContentFrame.Navigate(typeof(Bootloader_Driver), parent);
            }
            else if ((string)selectedItem.Tag == "Rec_Reboot")
            {
                ContentFrame.Navigate(typeof(Rec_Reboot));
            }
            else if ((string)selectedItem.Tag == "Setting")
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
        }

        // 侧边导航 返回按钮
        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;
        }
    }
}
