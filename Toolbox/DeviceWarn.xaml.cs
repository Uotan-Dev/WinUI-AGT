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
                warntxt = "С��6����WiFi��GPU��LTE����ص�Ӳ����δ�ɹ����������������칫������Ϸ�������ʣ��ʲ��Ƽ���װWindows�������濼�ǣ��رճ��򼴿ɷ������ء�\n\rע�⣺����835�豸�޷���װWindows11 22H2���Ժ�汾��";
            }
            if (Global.device == "MIX2")
            {
                warntxt = "MIX2����ȱ�������������ֻ���������Ӳ������������������������ȣ���Windowsϵͳ���޷�ʹ�á������濼�ǣ��رճ��򼴿ɷ������ء�\n\r*����835�豸�޷���װWindows11 22H2���Ժ�汾��";
            }
            if (Global.device == "K20Pro")
            {
                warntxt = "Redmi K20 Pro��USB�Դ������⣬����ʱ����UEFIʱ������ʹ��USB��ĿǰҲ��֧����Ļ�����������濼�ǣ��رճ��򼴿ɷ������ء�";
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
