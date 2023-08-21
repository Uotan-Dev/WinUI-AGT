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
using System.Threading;
using System.Diagnostics;

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

        // ������Դ

        private async void MindowsDownloadResource(object sender, RoutedEventArgs e)
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
                                    DeviceWarn form2 = new DeviceWarn();
                                    form2.Activate();
                                }
                                else
                                {
                                    DownloadResource form2 = new DownloadResource();
                                    form2.Activate();
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
                                        DeviceWarn form2 = new DeviceWarn();
                                        form2.Activate();
                                    }
                                    else
                                    {
                                        DownloadResource form2 = new DownloadResource();
                                        form2.Activate();
                                    }
                                }
                            }
                        }
                        else
                        {
                            _ = Parent.ShowDialog("δ�ܻ�ȡ�������ӣ���ȷ��������������������");
                        }
                    }
                }
                else
                {
                    _ = Parent.ShowDialog("��ѡ����ͣ�");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�ļ��ж�ʧ��");
            }
        }

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
                                    DownloadResource downlaoder = new();
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

        private async void FixOobeError(object sender, RoutedEventArgs e)
        {
            Setdevice();
            if (Global.device != "")
            {
                if (Mindows.Whoami("").IndexOf("system") != -1)
                {
                    bool result = await Parent.ShowDialogYesOrNo("���Ƚ��豸���������ģʽ��");
                    if (result == true)
                    {
                        Global.moreability = "oobeerror";
                        MindowsWidget form = new();
                        form.Activate();
                    }
                }
                else
                {
                    bool result = await Parent.ShowDialogYesOrNo("��ǰ����Ȩ�޲��������Զ�����Ӧ�ò�����Ȩ�ޣ�");
                    if (result == true)
                    {
                        string shell = string.Format(@"-U:S -P:E -M:S -CurrentDirectory:{0} {1}\Toolbox.exe", exepath, exepath);
                        Mindows.NSudoLC(shell);
                    }
                }
            }
            else
            {
                _ = Parent.ShowDialog("δѡ����ͣ�");
            }
        }

        private async void ChangeUsbMode(object sender, RoutedEventArgs e)
        {
            Setdevice();
            if (Global.device != "")
            {
                bool result = await Parent.ShowDialogYesOrNo("���Ƚ��豸���������ģʽ��");
                if (result == true)
                {
                    Global.moreability = "usbmode";
                    MindowsWidget form = new MindowsWidget();
                    form.Activate();
                }
            }
            else
            {
                _ = Parent.ShowDialog("δѡ����ͣ�");
            }
        }

        private async void LoadRegister(object sender, RoutedEventArgs e)
        {
            string reg = Mindows.Reg(@"unload HKEY_LOCAL_MACHINE\Mindows");
            if (reg.IndexOf("��������") != -1 || reg.IndexOf("parameter is incorrect") != -1)
            {
                bool result = await Parent.ShowDialogYesOrNo("δ�ҵ����ص�ע����Ƿ���й��أ�");
                if (result == true)
                {
                    Global.moreability = "loadreg";
                    MindowsWidget form = new();
                    form.Activate();
                }
            }
            else if (reg.IndexOf("�ܾ�����") != -1 || reg.IndexOf("Access is denied") != -1)
            {
                _ = Parent.ShowDialog("Ȩ�޲��㣬�����������ٳ��Թ��أ�");
            }
            else if (reg.IndexOf("�����ɹ����") != -1 || reg.IndexOf("operation completed successfully") != -1)
            {
                _ = Parent.ShowDialog("ж��ע���ɹ���");
            }
            else
            {
                _ = Parent.ShowDialog("δ֪��������ϵ������" + reg);
            }
        }


        // �ļ�ѡ����
        private async void PickUefiFileClick(object sender, RoutedEventArgs e)
        {
            PickUefiFileOutputTextBlock.Text = "";
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            nint windowHandle = WindowNative.GetWindowHandle(App.Window);
            InitializeWithWindow.Initialize(openPicker, windowHandle);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null) PickUefiFileOutputTextBlock.Text = file.Path;
            else PickUefiFileOutputTextBlock.PlaceholderText = "������ȡ��";
        }

        // ��ʱ����
        private async void BootCore(string mode)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                if (PickUefiFileOutputTextBlock.Text != "")
                {
                    BootUefiButton.IsEnabled = false;
                    FlashUefiBootButton.IsEnabled = false;
                    FlashUefiBootAButton.IsEnabled = false;
                    FlashUefiBootBButton.IsEnabled = false;
                    FlashUefiRecoveryButton.IsEnabled = false;
                    string file = PickUefiFileOutputTextBlock.Text;
                    string shell = string.Format(mode, @" {0}", file);
                    string sfstring = await ADBHelper.Fastboot(shell);
                    int sf = sfstring.IndexOf("FAILED");
                    if (sf == -1)
                    {
                        _ = Parent.ShowDialog("�����ɹ���");
                    }
                    else
                    {
                        _ = Parent.ShowDialog("����ʧ�ܣ�");
                    }
                    BootUefiButton.IsEnabled = true;
                    FlashUefiBootButton.IsEnabled = true;
                    FlashUefiBootAButton.IsEnabled = true;
                    FlashUefiBootBButton.IsEnabled = true;
                    FlashUefiRecoveryButton.IsEnabled = true;
                }
                else
                {
                    _ = Parent.ShowDialog("��ѡ��UEFI�ļ���");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�����Fastbootģʽ��");
            }
        }

        private void BootUefiClick(object sender, RoutedEventArgs e) { BootCore(@"boot"); }
        private void FlashUefiBootClick(object sender, RoutedEventArgs e) { BootCore(@"flash boot"); }
        private void FlashUefiBootAClick(object sender, RoutedEventArgs e) { BootCore(@"flash boot_a"); }
        private void FlashUefiBootBClick(object sender, RoutedEventArgs e) { BootCore(@"flash boot_b"); }
        private void FlashUefiRecoveryClick(object sender, RoutedEventArgs e) { BootCore(@"flash recovery"); }

        // �ָ�

        private async void RecoveryCore(string mode)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                string filepath = "";
                string filepath1 = String.Format(@"{0}\backup\boot.img", exepath);
                string filepath2 = String.Format(@"{0}\backup\boot_a.img", exepath);
                string filepath3 = String.Format(@"{0}\backup\boot_b.img", exepath);
                if (File.Exists(filepath1))
                {
                    filepath = filepath1;
                }
                else if (File.Exists(filepath2))
                {
                    filepath = filepath2;
                }
                else if (File.Exists(filepath3))
                {
                    filepath = filepath3;
                }
                if (filepath != "")
                {
                    RecoverBootButton.IsEnabled = false;
                    RecoverBootAButton.IsEnabled = false;
                    RecoverBootBButton.IsEnabled = false;
                    string shell = string.Format("flash ",mode ," {0}", filepath);
                    string sfstring = await ADBHelper.Fastboot(shell);
                    int sf = sfstring.IndexOf("FAILED");
                    if (sf == -1)
                    {
                        _ = Parent.ShowDialog("�ָ��ɹ���");
                    }
                    else
                    {
                        _ = Parent.ShowDialog("�ָ�ʧ�ܣ�");
                    }
                    RecoverBootButton.IsEnabled = true;
                    RecoverBootAButton.IsEnabled = true;
                    RecoverBootBButton.IsEnabled = true;
                }
                else
                {
                    _ = Parent.ShowDialog("δ�ҵ������ļ���");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�����Fastbootģʽ��");
            }
        }

        private void RecoverBootClick(object sender, RoutedEventArgs e) { RecoveryCore(@"boot"); }
        private void RecoverBootToAClick(object sender, RoutedEventArgs e) { RecoveryCore(@"boot_a"); }
        private void RecoverBootToBClick(object sender, RoutedEventArgs e) { RecoveryCore(@"boot_b"); }

        private async void RecoverPart(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Recovery")
            {
                Setdevice();
                if (Global.device != "")
                {
                    string mindowspath = String.Format(@"{0}\data\mindows\img\recovery.img", exepath);
                    if (File.Exists(mindowspath))
                    {
                        bool result = await Parent.ShowDialogYesOrNo("�˲�������ȫ�Ƴ�Windows���֣�\n�Ƿ������");
                        if (result == true)
                        {
                            Global.moreability = "repart";
                            MindowsWidget form = new();
                            form.Activate();
                        }
                    }
                    else
                    {
                        _ = Parent.ShowDialog("δ������Դ��");
                    }
                }
                else
                {
                    _ = Parent.ShowDialog("δѡ����ͣ�");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�뽫�豸����Recoveryģʽ��ִ�У�");
            }
        }

        // ��ʽ��

        private async void FormatCore(string mode)
        {
            Setdevice();
            if (Global.device != "")
            {
                bool result = await Parent.ShowDialogYesOrNo("���Ƚ��豸���������ģʽ��\n�Ƿ������");
                if (result == true)
                {
                    Global.moreability = mode;
                    MindowsWidget form = new();
                    form.Activate();
                }
            }
            else
            {
                _ = Parent.ShowDialog("δѡ����ͣ�");
            }
        }

        private void FormatESP(object sender, RoutedEventArgs e) { FormatCore("formatesp"); }
        private void FormatWindows(object sender, RoutedEventArgs e) { FormatCore("formatwin"); }


        // �޸�

        private async void FixEsp(object sender, RoutedEventArgs e)
        {
            Setdevice();
            if (Global.device != "")
            {
                bool result = await Parent.ShowDialogYesOrNo("���Ƚ��豸���������ģʽ��\n\rע�⣺�˹��̲����������ǩ����֤!");
                if (result == true)
                {
                    Global.moreability = "fixesp";
                    MindowsWidget form = new();
                    form.Activate();
                }
            }
            else
            {
                _ = Parent.ShowDialog("δѡ����ͣ�");
            }
        }

        private void DisableDriverSign(object sender, RoutedEventArgs e) { FormatCore("disdsv"); }

        private async void LabelEfi(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Recovery")
            {
                _ = ADBHelper.ADB("push bin/parted /tmp/");
                _ = ADBHelper.ADB("shell chmod +x /tmp/parted");
                string parttable = await ADBHelper.ADB("shell /tmp/parted /dev/block/sda print");
                if (parttable.IndexOf("esp") != -1)
                {
                    int espno = Mindows.Onlynum(Mindows.Partno(parttable, "esp"));
                    string shell = string.Format("shell /tmp/parted /dev/block/sda set {0} esp on", espno);
                    _ = ADBHelper.ADB(shell);
                    _ = Parent.ShowDialog("ִ����ɣ�");
                }
                else
                {
                    _ = Parent.ShowDialog("δ�ҵ�ESP������");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�뽫�豸����Recoveryģʽ��ִ�У�");
            }
        }

        private async void FlashDevcfg(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                string filepath = String.Format(@"{0}\data\mindows\img\devcfg.img", exepath);
                if (File.Exists(filepath))
                {
                    FlashDevcfgButton.IsEnabled = false;
                    string sfstring = await ADBHelper.Fastboot(@"flash devcfg_ab data\mindows\img\devcfg.img");
                    int sf = sfstring.IndexOf("FAILED");
                    if (sf == -1)
                    {
                        _ = Parent.ShowDialog("ˢ��ɹ���");
                    }
                    else
                    {
                        _ = Parent.ShowDialog("ˢ��ʧ�ܣ�");
                    }
                    FlashDevcfgButton.IsEnabled = true;
                }
                else
                {
                    _ = Parent.ShowDialog("����豸����ˢ����ļ���δ������Դ��");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�����Fastbootģʽ��");
            }
        }

        private void InstallDrive(object sender, RoutedEventArgs e) { FormatCore("installdrive"); }

        private void OpenDism(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"data\dism\Dism++x64.exe");
        }

        private async void EnterMassMode(object sender, RoutedEventArgs e)
        {
            Parent.CheckconAsync();
            if (Parent.ConnInfoText == "Fastboot")
            {
                string filepath = String.Format(@"{0}\data\mindows\img\automass.img", exepath);
                if (File.Exists(filepath))
                {
                    bool result = await Parent.ShowDialogYesOrNo("��ʱ�������ڷ��գ���ȷ����ǰBootΪ��׿Boot��\n�Ƿ������");
                    if (result == true)
                    {
                        EnterMassModeButton.IsEnabled = false;
                        string sfstring = await ADBHelper.Fastboot(@"boot data\mindows\img\automass.img");
                        int sf = sfstring.IndexOf("FAILED");
                        if (sf == -1)
                        {
                            _ = Parent.ShowDialog("�����ɹ���");
                        }
                        else
                        {
                            _ = Parent.ShowDialog("����ʧ�ܣ�");
                        }
                        EnterMassModeButton.IsEnabled = true;
                    }
                }
                else
                {
                    _ = Parent.ShowDialog("δ������Դ����");
                }
            }
            else
            {
                _ = Parent.ShowDialog("�����Fastbootģʽ��");
            }
        }

        private async void ReinstallWindows(object sender, RoutedEventArgs e)
        {
            Setdevice();
            if (Global.device != "")
            {
                bool result = await Parent.ShowDialogYesOrNo("���Ƚ��豸���������ģʽ����ȷ���ѽ�ESP��Windows������ʽ����\r\nע�⣺�˹��̲��ᰲװ��������UEFI��Ҳ�����������ǩ����֤��\n�Ƿ������");
                if (result == true)
                {
                    Global.moreability = "installwin";
                    MindowsWidget form = new();
                    form.Activate();
                }
            }
            else
            {
                _ = Parent.ShowDialog("δѡ����ͣ�");
            }
        }

        // ����

        private void JoinGroup(object sender, RoutedEventArgs e) { Mindows.OpenDefaultBrowserUrl("https://jq.qq.com/?_wv=1027&k=xPu36sWg"); }
        private void OpenBackupFolder(object sender, RoutedEventArgs e)
        {
            string filepath = String.Format(@"{0}\backup", exepath);
            Process.Start("Explorer.exe", filepath);
        }
        private void OfficialWebsite(object sender, RoutedEventArgs e) { Mindows.OpenDefaultBrowserUrl("https://mindows.cn"); }
        private void OfficialVideo(object sender, RoutedEventArgs e) { Mindows.OpenDefaultBrowserUrl("https://www.bilibili.com/video/BV1oY4y167SA"); }
        private void DownloadRecommendImage(object sender, RoutedEventArgs e) { Mindows.OpenDefaultBrowserUrl("https://www.123pan.com/s/8eP9-BkTGA"); }
        private void DownloadMoreImage(object sender, RoutedEventArgs e) { Mindows.OpenDefaultBrowserUrl("https://uupdump.cn"); }

    }
}