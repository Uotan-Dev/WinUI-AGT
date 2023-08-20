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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Toolbox
{
    public sealed partial class DeviceWarn : Window
    {
        public DeviceWarn()
        {
            this.InitializeComponent();
            string warntxt = "";
            if (Global.device == "MI6")
            {
                warntxt = "小米6由于WiFi，GPU，LTE，电池等硬件暂未成功驱动，无论用来办公还是游戏都不合适，故不推荐安装Windows。请认真考虑，关闭程序即可放弃下载。\n\r注意：骁龙835设备无法安装Windows11 22H2及以后版本！";
            }
            if (Global.device == "MIX2")
            {
                warntxt = "MIX2由于缺少配套驱动，手机几乎所有硬件（包括触屏，声音，网络等）在Windows系统均无法使用。请认真考虑，关闭程序即可放弃下载。\n\r*骁龙835设备无法安装Windows11 22H2及以后版本！";
            }
            if (Global.device == "K20Pro")
            {
                warntxt = "Redmi K20 Pro的USB仍存在问题，仅临时启动UEFI时可正常使用USB！目前也不支持屏幕触摸。请认真考虑，关闭程序即可放弃下载。";
            }
            ShowText.Text = warntxt;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            DownloadResource form2 = new DownloadResource();
            form2.Activate();
            Close();
        }
    }
}
