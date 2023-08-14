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

        //定义窗口
        public FlashRom()
        {
            this.InitializeComponent();
        }

        // Bootloader文件选择器
        private async void PickUnlockFileClick(object sender, RoutedEventArgs e)
        {
            PickUnlockFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickUnlockFileOutputTextBlock.Text = "已选择 " + file.Name;
            else PickUnlockFileOutputTextBlock.Text = "操作已取消";
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
                    int sf = sfstring.IndexOf("OKAY");
                    if (sf != -1)
                    {
                        _ = Parent.ShowDialog("解锁成功！");
                    }
                    else
                    {
                        _ = Parent.ShowDialog("解锁失败！");
                    }
                }
                else
                {
                    _ = Parent.ShowDialog("请选择解锁文件！");
                }
            }
            else
            {
                _ = Parent.ShowDialog("请进入Fastboot模式！");
            }
        }
    }
}
