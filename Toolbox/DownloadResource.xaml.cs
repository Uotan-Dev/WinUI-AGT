using System;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace Toolbox
{
    public sealed partial class DownloadResource : Window
    {
        public DownloadResource()
        {
            this.InitializeComponent();

            Mindows.Disdevice();

            string link = "";
            if (Global.havedrv)
            {
                ShowText.Text = "正在获取下载链接";
                link = Mindows.GetLink(Global.drivelink1);
                if (link != "")
                {
                    ShowText.Text = "正在下载驱动程序";
                    _ = Mindows.DownloadFile(link, @"data\mindows\driver.7z.001", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("获取下载链接失败");
                    Close();
                }
            }
            if (Global.drivelink2 != "")
            {
                ShowText.Text = "正在获取下载链接";
                link = Mindows.GetLink(Global.drivelink2);
                if (link != "")
                {
                    ShowText.Text = "正在下载驱动程序②";
                    _ = Mindows.DownloadFile(link, @"data\mindows\driver.7z.002", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("获取下载链接失败");
                    Close();
                }
            }
            if (Global.drivelink3 != "")
            {
                ShowText.Text = "正在获取下载链接";
                link = Mindows.GetLink(Global.drivelink3);
                if (link != "")
                {
                    ShowText.Text = "正在下载驱动程序③";
                    _ = Mindows.DownloadFile(link, @"data\mindows\driver.7z.003", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("获取下载链接失败");
                    Close();
                }
            }
            if (Global.imglink != "")
            {
                ShowText.Text = "正在获取下载链接";
                link = Mindows.GetLink(Global.imglink);
                if (link != "")
                {
                    ShowText.Text = "正在下载镜像文件";
                    _ = Mindows.DownloadFile(link, @"data\mindows\img.7z.001", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("获取下载链接失败");
                    Close();
                }
            }
            ShowText.Text = "正在解压资源";
            int unzipcheck2 = Mindows.Unzip(new DirectoryInfo(@"data\mindows"), "").IndexOf("Ok");
            if (unzipcheck2 != -1)
            {
                File.Delete(@"data\mindows\driver.7z.001");
                File.Delete(@"data\mindows\driver.7z.002");
                File.Delete(@"data\mindows\driver.7z.003");
                File.Delete(@"data\mindows\img.7z.001");
                ShowText.Text = "下载完成，您可以关闭本窗口！";
            }
            else
            {
                _ = ShowDialog("资源解压失败");
                ShowText.Text = "资源解压失败";
            }
        }

        // 消息弹窗 = MessageBox
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
    }
}
