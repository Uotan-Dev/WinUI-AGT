using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using System.Security.Principal;
using System.IO;
using System.Threading.Tasks;

namespace Toolbox
{
    public sealed partial class MindowsTool : Page
    {
        public new MainWindow Parent { get; set; }

        public class MindowsToolParameter
        {
            public MainWindow Parent { get; set; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is MindowsToolParameter parameter)
            {
                Parent = parameter.Parent;
            }
        }

        //���崰��
        public MindowsTool()
        {
            this.InitializeComponent();
        }

        // �ļ�ѡ����
        /*private async void PickUnlockFileClick(object sender, RoutedEventArgs e)
        {
            PickUnlockFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickUnlockFileOutputTextBlock.Text = file.Path;
            else PickUnlockFileOutputTextBlock.PlaceholderText = "������ȡ��";
        }*/

        // ��һ����װ����
        private async void OpenMindowsClick(object sender, RoutedEventArgs e)
        {
            Setdevice();
            Parent.CheckconAsync();
            Mindows.Disdevice();
            if (Parent.ConnInfoText == "Fastboot")
            {
                string mindowspath1 = String.Format(@"{0}\data\mindows\img\uefi.img", exepath);
                string mindowspath2 = String.Format(@"{0}\data\mindows\img\automass.img", exepath);
                string mindowspath3 = String.Format(@"{0}\data\mindows\img\recovery.img", exepath);
                if (File.Exists(mindowspath1) && File.Exists(mindowspath2) && File.Exists(mindowspath3))
                {
                    if (Global.device != "")
                    {
                        bool result = await Parent.ShowDialogYesOrNo("��ȷ���豸�Ѹ�ʽ��Data������");
                        if (result == true)
                        {
                            bool resultdl = await Parent.ShowDialogYesOrNo("��ȷ�����Ƿ����������õ�\r\nWindows����");
                            if (resultdl == true)
                            {
                                _ = Parent.ShowDialog("��ӭʹ��Mindowsһ����װ���˹�����Ϊ�ֻ�ˢ��Windows�����ˢ���򵼣�������������ɰ�׿+Windows˫ϵͳ�Ĳ���ϣ�����ܰ�����˳�����ˢ�����ڿ�ʼ֮ǰ������������Ķ��������ݣ�\r\n\r\n-�˹�����ȫ��ѣ� ��ֹ�κ������κ���ʽ���ã������������ڷ���������Զ��ˢ���ȡ���ֹ������ʽ�Ķ����׿Ǵ����\r\n-RenegadeProject��Ŀ��ȫ��ѣ���ֹ�κ������κ���ʽ���ã������������ڸ���Զ��ˢ�����ṩ�г�ˢ��ָ�����������ˢ������ˢ�����ߡ�ˢ���̵̳ȡ�\r\n-�緢��Υ�������涨����Ϊ�����QQ1330250642�ٱ�����л����֧�֡�\r\n\r\n-ˢ����ɺ��豸�н�ͬʱ���ڰ�׿��Windowsϵͳ�����ռ��һ���ְ�׿�Ŀռ䡣\r\n-ˢ��������豸���������ݣ��������ǰ���������ݱ��ݵ��豸֮��ĵط���\r\n-ʹ�ô˹�����Ҫ�Ƚ���BL������ȷ������豸BL���ѽ�����\r\n-ˢWindows��Ҫ�޸ķ������������Ϊ�޸Ĺ������������Ȼָ��ٷ��������ټ�����\r\n\r\n-ˢ�����ܵ����豸ϵͳ���޷�������\r\n-ˢ�����ܻ�Ӱ��ϵͳOTA���¹��ܣ�ˢ��������ʹ��ϵͳ���¡�\r\n-ˢ����Ը���������߲������������豸��Ҳ��Ϊ�κο��ܷ��������⸺��");
                                bool resultur = await Parent.ShowDialogYesOrNo("���棡\r\n��ȷ�����г����ʱ�����ˢ�������������뱣���ֳ�����ϵ������\r\n��װWindows���漰�޸ķ������˲������սϴ����ױ�ש��\r\n���޸ķ������޷�ʹ��MiFlash��Fastbootģʽˢ����\r\n\r\n��ȷ�������Ķ��������ݲ�ͬ����Ըˢ����");
                                if (resultur == true)
                                {
                                    MindowsInstall form2 = new();
                                    form2.Activate();
                                }
                            }
                            else Mindows.OpenDefaultBrowserUrl("https://www.123pan.com/s/8eP9-BkTGA");
                        }
                    }
                    else _ = Parent.ShowDialog("��ѡ����ͣ�");
                }
                else _ = Parent.ShowDialog("����������Դ��");
            }
            else _ = Parent.ShowDialog("�����Fastbootģʽ��");
        }

        //������Դ�������Ӵ���
        readonly string mix2simglink = "";
        readonly string mix2sdrivelink = "";
        readonly string mi8imglink = "";
        readonly string mi8drivelink = "";
        readonly string mix3imglink = "";
        readonly string mix3drivelink = "";
        readonly string pad5imglink = "";
        readonly string pad5drivelink1 = "";
        readonly string pad5drivelink2 = "";
        readonly string mi9imglink = "";
        readonly string mi9drivelink1 = "";
        readonly string mi9drivelink2 = "";
        readonly string mi9drivelink3 = "";
        readonly string mi6imglink = "";
        readonly string mi6drivelink = "";
        readonly string k20pdrivelink1 = "";
        readonly string k20pdrivelink2 = "";
        readonly string k20pdrivelink3 = "";
        readonly string k20pimglink = "";

        //��ȡ��������·��
        readonly string exepath = System.IO.Directory.GetCurrentDirectory();

        public void Setdevice()
        {
            string SelectedDevice = (ChooseModel.SelectedValue as ComboBoxItem)?.Content.ToString();
            if (SelectedDevice == "С��MIX2S")
            {
                Global.device = "MIX2S";
                Global.drivelink1 = mix2sdrivelink;
                Global.imglink = mix2simglink;
            }
            if (SelectedDevice == "С��8")
            {
                Global.device = "MI8";
                Global.drivelink1 = mi8drivelink;
                Global.imglink = mi8imglink;
            }
            if (SelectedDevice == "С��MIX3")
            {
                Global.device = "MIX3";
                Global.drivelink1 = mix3drivelink;
                Global.imglink = mix3imglink;
            }
            if (SelectedDevice == "С��ƽ��5")
            {
                Global.device = "Pad5";
                Global.drivelink1 = pad5drivelink1;
                Global.drivelink2 = pad5drivelink2;
                Global.imglink = pad5imglink;
            }
            if (SelectedDevice == "С��9")
            {
                Global.device = "MI9";
                Global.drivelink1 = mi9drivelink1;
                Global.drivelink2 = mi9drivelink2;
                Global.drivelink3 = mi9drivelink3;
                Global.imglink = mi9imglink;
            }
            if (SelectedDevice == "Redmi K20 Pro")
            {
                Global.device = "K20Pro";
                Global.drivelink1 = k20pdrivelink1;
                Global.drivelink2 = k20pdrivelink2;
                Global.drivelink3 = k20pdrivelink3;
                Global.imglink = k20pimglink;
            }
            if (SelectedDevice == "С��6")
            {
                Global.device = "MI6";
                Global.drivelink1 = mi6drivelink;
                Global.imglink = mi6imglink;
            }
            if (SelectedDevice == "С��MIX2")
            {
                Global.device = "MIX2";
            }
        }

        // ������Դ
        private async void DownloadResource(object sender, RoutedEventArgs e)
        {
            Setdevice();
            Mindows.Disdevice();
            string mindowspath = String.Format(@"{0}\data\mindows", exepath);
            if (Directory.Exists(mindowspath))
            {
                if (Global.device != "")
                {
                    bool stardownload = true;
                    try
                    {
                        File.Delete(@"data\mindows\driver.7z.001");
                        File.Delete(@"data\mindows\driver.7z.002");
                        File.Delete(@"data\mindows\driver.7z.003");
                        File.Delete(@"data\mindows\img.7z.001");
                    }
                    catch
                    {
                        _ = Parent.ShowDialog("ɾ����ʱ�ļ�ʧ�ܣ�������Ӧ�ú��ٳ������أ�");
                        stardownload = false;
                    }
                    if (stardownload)
                    {
                        mindowspath = String.Format(@"{0}\data\mindows\img", exepath);
                        if (Global.drivelink1 != "" || Global.imglink != "")
                        {
                            if (!Directory.Exists(mindowspath))
                            {
                                if (Global.warn)
                                {
                                    DeviceWarn warning = new DeviceWarn();
                                    warning.Activate();
                                }
                                else
                                {
                                    DownloadResource downlaoder = new DownloadResource();
                                    downlaoder.Activate();
                                }
                            }
                            else
                            {
                                bool result = await Parent.ShowDialogYesOrNo("����������Դ���Ƿ��������أ�");
                                if (result == true)
                                {
                                    Mindows.DeleteFolder(@"data\mindows\driver");
                                    Mindows.DeleteFolder(@"data\mindows\img");
                                    if (Global.warn)
                                    {
                                        DeviceWarn warning = new DeviceWarn();
                                        warning.Activate();
                                    }
                                    else
                                    {
                                        DownloadResource downlaoder = new DownloadResource();
                                        downlaoder.Activate();
                                    }
                                }
                            }
                        }
                        else _ = Parent.ShowDialog("δ�ܻ�ȡ�������ӣ���ȷ��������������������");
                    }
                }
                else _ = Parent.ShowDialog("��ѡ����ͣ�");
            }
            else _ = Parent.ShowDialog("�ļ��ж�ʧ��");
        }

        // ע������
        private async void SkipWebOobe(object sender, RoutedEventArgs e)
        {
            Setdevice();
            if (Global.device != "")
            {
                bool result = await Parent.ShowDialogYesOrNo("���Ƚ��豸���������ģʽ��");
                if (result == true)
                {
                    Global.moreability = "noweboobe";
                    MindowsWidget form = new();
                    form.Activate();
                }
            }
            else
            {
                _ = Parent.ShowDialog("δѡ����ͣ�");
            }
        }
    }
}
