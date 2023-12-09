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
                ShowText.Text = "���ڻ�ȡ��������";
                link = Mindows.GetLink(Global.drivelink1);
                if (link != "")
                {
                    ShowText.Text = "����������������";
                    _ = Mindows.DownloadFile(link, @"data\mindows\driver.7z.001", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("��ȡ��������ʧ��");
                    Close();
                }
            }
            if (Global.drivelink2 != "")
            {
                ShowText.Text = "���ڻ�ȡ��������";
                link = Mindows.GetLink(Global.drivelink2);
                if (link != "")
                {
                    ShowText.Text = "�����������������";
                    _ = Mindows.DownloadFile(link, @"data\mindows\driver.7z.002", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("��ȡ��������ʧ��");
                    Close();
                }
            }
            if (Global.drivelink3 != "")
            {
                ShowText.Text = "���ڻ�ȡ��������";
                link = Mindows.GetLink(Global.drivelink3);
                if (link != "")
                {
                    ShowText.Text = "�����������������";
                    _ = Mindows.DownloadFile(link, @"data\mindows\driver.7z.003", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("��ȡ��������ʧ��");
                    Close();
                }
            }
            if (Global.imglink != "")
            {
                ShowText.Text = "���ڻ�ȡ��������";
                link = Mindows.GetLink(Global.imglink);
                if (link != "")
                {
                    ShowText.Text = "�������ؾ����ļ�";
                    _ = Mindows.DownloadFile(link, @"data\mindows\img.7z.001", PraogressPercentBar, ProgressPercent);
                }
                else
                {
                    _ = ShowDialog("��ȡ��������ʧ��");
                    Close();
                }
            }
            ShowText.Text = "���ڽ�ѹ��Դ";
            int unzipcheck2 = Mindows.Unzip(new DirectoryInfo(@"data\mindows"), "").IndexOf("Ok");
            if (unzipcheck2 != -1)
            {
                File.Delete(@"data\mindows\driver.7z.001");
                File.Delete(@"data\mindows\driver.7z.002");
                File.Delete(@"data\mindows\driver.7z.003");
                File.Delete(@"data\mindows\img.7z.001");
                ShowText.Text = "������ɣ������Թرձ����ڣ�";
            }
            else
            {
                _ = ShowDialog("��Դ��ѹʧ��");
                ShowText.Text = "��Դ��ѹʧ��";
            }
        }

        // ��Ϣ���� = MessageBox
        public async Task ShowDialog(string content)
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

            await dialog.ShowAsync();
        }
    }
}
