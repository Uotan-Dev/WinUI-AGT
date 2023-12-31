using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using Microsoft.UI.Composition.SystemBackdrops;
using System.Threading.Tasks;
using static Toolbox.BootloaderDriver;
using static Toolbox.Rec_Reboot;
using static Toolbox.FlashRom;
using Windows.ApplicationModel.Activation;
using static Toolbox.MoreFlash;
using static Toolbox.MindowsTool;
using Windows.ApplicationModel.Core;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Toolbox
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            var parent = new BootloaderDriverParameter { Parent = this };

            Title = "Android 极客工具箱";
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();

            ContentFrame.Navigate(
                typeof(BootloaderDriver),
                parent,
                new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo()
            );

            SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            CheckconAsync();

            Closed += CurrentWindowClosed;
        }

        private void CurrentWindowClosed(object sender, WindowEventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("adb");
            if (processes.Length > 0)
            {
                foreach (Process process in processes)
                {
                    process.Kill();
                    process.Dispose();
                }
            }
        }

        public string GetAppTitleFromSystem()
        {
            return Windows.ApplicationModel.Package.Current.DisplayName;
        }

        // 消息弹窗
        public async Task ShowDialog(string content)
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

            await dialog.ShowAsync();
        }

        // 返回布尔值的消息弹窗
        public async Task<bool> ShowDialogYesOrNo(string content)
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

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary) return true;
            else return false;
        }

        // 搜索框后端
        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }
        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            _ = args.QueryText;  //输入的文本
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
        public async void CheckconAsync()
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingBar.IsIndeterminate = true;
            string FastbootDevices = await ADBHelper.Fastboot("devices");
            string adbcheckstring = await ADBHelper.ADB("devices");
            if (FastbootDevices != "") conninfo.Text = "Fastboot";
            else conninfo.Text = "未连接";

            int adbcheck = adbcheckstring.IndexOf("recovery");
            if (adbcheck != -1) conninfo.Text = "Recovery";
            int adbcheck2 = adbcheckstring.IndexOf("sideload");
            if (adbcheck2 != -1) conninfo.Text = "Sideload";
            int adbcheck3 = adbcheckstring.IndexOf("	device");
            if (adbcheck3 != -1) conninfo.Text = "系统";
            int check9008 = Mindows.Devcon("find usb*").IndexOf("QDLoader");
            if (check9008 != -1) conninfo.Text = "9008";
            int check901d = Mindows.Devcon("find usb*").IndexOf("901D (");
            if (check901d != -1) conninfo.Text = "901D";
            int check900e = Mindows.Devcon("find usb*").IndexOf("900E");
            if (check900e != -1) conninfo.Text = "900E";
            int check9091 = Mindows.Devcon("find usb*").IndexOf("9091 (");
            if (check9091 != -1) conninfo.Text = "9091";

            if (conninfo.Text == "Fastboot")
            {
                string unlockedstring = await ADBHelper.Fastboot("getvar unlocked");
                int unlocked = unlockedstring.IndexOf("yes");
                if (unlocked != -1)
                {
                    BLinfo.Text = "已解锁";
                }
                int locked = unlockedstring.IndexOf("no");
                if (locked != -1)
                {
                    BLinfo.Text = "未解锁";
                    _ = ShowDialog("您的设备未解锁BootLoader！\n\r大部分功能将无法使用！");
                }
                string productinfos = await ADBHelper.Fastboot("getvar product");
                string product = Mindows.GetProductID(productinfos);
                if (product != null)
                {
                    productinfo.Text = product;
                }
                string active = await ADBHelper.Fastboot("getvar current-slot");
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
            LoadingBar.IsIndeterminate = false;
            LoadingBar.Visibility = Visibility.Collapsed;
        }
        public string ConnInfoText
        {
            get { return conninfo.Text; }
            set { conninfo.Text = value; }
        }
        public ProgressBar LoadingBarArgs
        {
            get { return LoadingBar; }
            set { LoadingBar = value; }
        }

        // 检查链接按钮
        private void CheckconnClick(object sender, RoutedEventArgs e)
        {
            CheckconAsync();
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender,
                      NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                ContentFrame.Navigate(typeof(SettingsPage), null, args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null && (args.InvokedItemContainer.Tag != null))
            {
                Type newPage = Type.GetType(args.InvokedItemContainer.Tag.ToString());
                var newPageParameterString = args.InvokedItemContainer.Tag.ToString() + "+"+Regex.Replace((args.InvokedItemContainer.Tag.ToString()) + "Parameter", @"^Toolbox\.", "");
                Type newPageParameter = Type.GetType(newPageParameterString);
                var parent = Activator.CreateInstance(newPageParameter);
                newPageParameter.GetProperty("Parent").SetValue(parent, this);
                ContentFrame.Navigate(
                       newPage,
                       parent,
                       args.RecommendedNavigationTransitionInfo
                       );
            }
        }

        // 侧边导航 返回按钮
        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack) ContentFrame.GoBack();
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                NavigationViewControl.SelectedItem = (NavigationViewItem)NavigationViewControl.SettingsItem;
            }
            else if (ContentFrame.SourcePageType != null)
            {
                NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(ContentFrame.SourcePageType.FullName.ToString()));
            }

            NavigationViewControl.Header = ((NavigationViewItem)NavigationViewControl.SelectedItem)?.Content?.ToString();
        }
    }
}
