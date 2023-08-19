using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Toolbox
{
    public sealed partial class SetWinPart : Window
    {
        public SetWinPart()
        {
            this.InitializeComponent();
            SetWinPartLoad();
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

        readonly int maxsize = Global.datasize - 3;
        readonly int minsize = 20;
        private async void SetWinPartLoad()
        {
            if (Global.datasizeunit != "GB")
            {
                await ShowDialog("����Data��������GB���޷���װWindows��");
                Close();
            }
            else
            {
                AvailableSpaceText.Text = Global.datasize.ToString();
                string wineange = string.Format("20GB-{0}GB", maxsize);
                SpaceRangeText.Text = wineange;
            }
        }

        private async void ConfirmClick(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            if (toggleSwitch.IsOn)
            {
                if (WindowsSizeTextBox.Text != "" || SharePartSizeTextBox.Text != "")
                {
                    int winsize = Mindows.Onlynum(WindowsSizeTextBox.Text);
                    int sharesize = Mindows.Onlynum(SharePartSizeTextBox.Text);
                    int totalsize = winsize + sharesize;
                    if (totalsize <= maxsize && totalsize >= minsize)
                    {
                        Global.winsize = winsize;
                        Global.sharepartsize = sharesize;
                        Global.mksharepart = true;
                        this.Close();
                    }
                    else
                    {
                        await ShowDialog("���趨�ķ�����С���ڷ�Χ�ڣ��������趨");
                    }
                }
                else
                {
                    await ShowDialog("������Windows���������������С");
                }
            }
            else
            {
                if (WindowsSizeTextBox.Text != "")
                {
                    int winsize = Mindows.Onlynum(WindowsSizeTextBox.Text);
                    if (winsize <= maxsize && winsize >= minsize)
                    {
                        Global.winsize = winsize;
                        this.Close();
                    }
                    else
                    {
                        await ShowDialog("���趨�ķ�����С���ڷ�Χ�ڣ��������趨");
                    }
                }
                else
                {
                    await ShowDialog("������Windows������С");
                }
            }
        }

        private void CreateSharePartToggled(object sender, EventArgs e)
        {
            ToggleSwitch toggleSwitch = (ToggleSwitch)sender;
            if (toggleSwitch.IsOn)
            {
                SharePartSizeTextBox.IsEnabled = true;
            }
            else
            {
                SharePartSizeTextBox.IsEnabled = false;
            }
        }
    }
}
