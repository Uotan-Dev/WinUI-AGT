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
        // ����������
        public MainWindow()
        {
            this.InitializeComponent();
            Title = "Android ���͹�����";
            SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();

            // ����һ����ʱ����������ĸ����
            var parent = new BootloaderDriverParameter { Parent = this };
            ContentFrame.Navigate(typeof(Bootloader_Driver), parent);
        }
        public string GetAppTitleFromSystem()
        {
            return Windows.ApplicationModel.Package.Current.DisplayName;
        }

        // ��Ϣ���� = MessageBox
        public async void ShowDialog(string content)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = this.PageContainer.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "��ʾ",
                PrimaryButtonText = "ȷ��",
                CloseButtonText = "ȡ��",
                DefaultButton = ContentDialogButton.Primary,
                Content = new ContentDialogContent(content)
            };

            _ = await dialog.ShowAsync();
        }
        public void ShowDialogClick(object sender, RoutedEventArgs e)
        {
            ShowDialog("Congratulations!");
        }

        
        // ��������
        private void SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }
        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string txt = args.QueryText;  //������ı�
            if (args.ChosenSuggestion != null)
            {
                //����ʾ����ѡ��ĳһ��ʱ����
            }
            else
            {
                //�û�������ʱ�ûس����ߵ���ұ߰�ťȷ������ʱ����
            }
        }


        // ������Ӻ��� - ȫ��
        public void Checkcon()
        {
            if (ADBHelper.Fastboot("devices") != "")
            {
                conninfo.Text = "Fastboot";
            }
            else
            {
                conninfo.Text = "δ����";
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
                conninfo.Text = "ϵͳ";
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
                    BLinfo.Text = "�ѽ���";
                }
                int locked = ADBHelper.Fastboot("getvar unlocked").IndexOf("no");
                if (locked != -1)
                {
                    BLinfo.Text = "δ����";
                    //Dialog_Display("�����豸δ����BootLoader��\n\r�󲿷ֹ��ܽ��޷�ʹ�ã�");
                    ShowDialog("�����豸δ����BootLoader��\n\r�󲿷ֹ��ܽ��޷�ʹ�ã�");
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
                    VABinfo.Text = "A��λ";
                }
                else if (active.IndexOf("current-slot: b") != -1)
                {
                    VABinfo.Text = "B��λ";
                }
                else if (active.IndexOf("FAILED") != -1)
                {
                    VABinfo.Text = "A-Only�豸";
                }
            }
            else
            {
                BLinfo.Text = "δ֪";
                VABinfo.Text = "δ֪";
                productinfo.Text = "δ֪";
            }
        }
        public string ConnInfoText
        {
            get { return conninfo.Text; }
            set { conninfo.Text = value; }
        }

        // ������Ӱ�ť
        private void CheckconnClick(object sender, RoutedEventArgs e)
        {
            Checkcon();
        }

        // ��ߵ�������л�
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

        // ��ߵ��� ���ذ�ť
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
