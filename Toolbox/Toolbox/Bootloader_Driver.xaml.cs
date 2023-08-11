using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Toolbox;
using Microsoft.UI.Xaml.Navigation;
using System.Reflection.Metadata;
using System.Threading.Tasks;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Toolbox
{
    public sealed partial class Bootloader_Driver : Page
    {
        public MainWindow parent { get; set; }

        public class BootloaderDriverParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is BootloaderDriverParameter parameter)
            {
                parent = parameter.Parent;
            }
        }

        //定义窗口
        public Bootloader_Driver()
        {
            this.InitializeComponent();
        }

        // Bootloader文件选择器
        private async void PickUnlockFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            PickUnlockFileOutputTextBlock.Text = "";

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            nint windowHandle = WindowNative.GetWindowHandle(App.m_window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                PickUnlockFileOutputTextBlock.Text = "Picked file: " + file.Name;
            }
            else
            {
                PickUnlockFileOutputTextBlock.Text = "Operation cancelled.";
            }
        }

        // Bootloader解锁按钮
        private void UnlockNowButton_Click(object sender, RoutedEventArgs e)
        {
            parent.Checkcon();
            if (parent.ConnInfoText == "Fastboot")
            {
                if (PickUnlockFileOutputTextBlock.Text != "")
                {
                    string file = PickUnlockFileOutputTextBlock.Text;
                    string shell = string.Format("flash unlock {0}", file);
                    string shell2 = "oem unlock-go";
                    ADBHelper.Fastboot(shell);
                    int sf = ADBHelper.Fastboot(shell2).IndexOf("OKAY");
                    if (sf != -1)
                    {
                        parent.ShowDialog("解锁成功！");
                    }
                    else
                    {
                        parent.ShowDialog("解锁失败！");
                    }
                }
                else
                {
                    parent.ShowDialog("请选择解锁文件！");
                }
            }
            else
            {
                parent.ShowDialog("请进入Fastboot模式！");
            }
        }


        private void RelockBootloaderButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
