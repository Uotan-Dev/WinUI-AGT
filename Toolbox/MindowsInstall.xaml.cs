using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Toolbox
{
    public sealed partial class MindowsInstall : Window
    {
        public MindowsInstall()
        {
            this.InitializeComponent();
            MindowsInstallShown();
        }

        public Process process = null;

        public void Diskpart(string shell)
        {
            Process process = new Process();
            process.StartInfo.FileName = "diskpart.exe";
            process.StartInfo.WorkingDirectory = ".";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            process.Start();
            process.StandardInput.WriteLine(shell);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }


        public void Fastboot(string fb)//Fastbootʵʱ���
        {
            string cmd = @"bin\fastboot.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb);
            fastboot.CreateNoWindow = true;
            fastboot.UseShellExecute = false;
            fastboot.RedirectStandardOutput = true;
            fastboot.RedirectStandardError = true;
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        public void ADB(string fb)//ADBʵʱ���
        {
            string cmd = @"bin\adb.exe";
            ProcessStartInfo adb = null;
            adb = new ProcessStartInfo(cmd, fb);
            adb.CreateNoWindow = true;
            adb.UseShellExecute = false;
            adb.RedirectStandardOutput = true;
            adb.RedirectStandardError = true;
            Process f = Process.Start(adb);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                StringBuilder sb = new(this.ShellShow.Text);
                this.ShellShow.Text = sb.AppendLine(outLine.Data).ToString();
                this.ShellShow.SelectionStart = this.ShellShow.Text.Length;
                var scrollViewer = FindChild<ScrollViewer>(ShellShow);
                scrollViewer.ChangeView(null, scrollViewer.ExtentHeight, null);
            }
        }

        // ��VisualTree�в���ָ�����͵���Ԫ��
        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;
                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        public void Dism(string fb)
        {
            string cmd = @"bin\Dism.bat";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb);
            fastboot.CreateNoWindow = true;
            fastboot.UseShellExecute = false;
            fastboot.RedirectStandardOutput = true;
            fastboot.RedirectStandardError = true;
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        public void Bcdboot(string fb)
        {
            string cmd = @"bin\bcdboot.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb);
            fastboot.CreateNoWindow = true;
            fastboot.UseShellExecute = false;
            fastboot.RedirectStandardOutput = true;
            fastboot.RedirectStandardError = true;
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        public void Bcdedit(string fb)
        {
            string cmd = @"bin\bcdedit.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb);
            fastboot.CreateNoWindow = true;
            fastboot.UseShellExecute = false;
            fastboot.RedirectStandardOutput = true;
            fastboot.RedirectStandardError = true;
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

        //ʵʱ������˽���

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

        //һЩȫ�ֱ���
        string output;//���


        //һЩֻ��д����ĺ���
        public void WriteShow()//������������д��txt
        {
            output = ShellShow.Text;
            Mindows.Write(@"log\disk.txt", output);
        }

        public int Findsda()//����Sda
        {
            Diskpart("list disk \r\nexit");
            WriteShow();
            ShellShow.Text = "";
            char[] charSeparators = new char[] { ' ' };
            string[] parts = output.Split('\n');
            if (parts.Length <= 4)
            {
                return 0;
            }
            else
            {
                int i = parts.Length - 4;
                string[] lastdisk = parts[i].Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); ;
                string disknum = Regex.Replace(lastdisk[1], @"[^0-9]+", "");
                int num = int.Parse(disknum);
                int totaldisk = num + 1;
                int a;
                for (a = 0; a < totaldisk; a++)
                {
                    string shell = String.Format("select disk {0} \r\ndetail disk \r\nexit", a);
                    Diskpart(shell);
                    WriteShow();
                    int issda = output.IndexOf("sda");
                    if (issda != -1)
                    {
                        break;
                    }
                    ShellShow.Text = "";
                }
                if (a == totaldisk)
                {
                    return 0;
                }
                else
                {
                    ShellShow.Text = "";
                    return a;
                }
            }
        }

        public int FindLinux()
        {
            Diskpart("list disk\r\nexit");
            WriteShow();
            ShellShow.Text = "";
            char[] charSeparators = new char[] { ' ' };
            string[] parts = output.Split('\n');
            if (parts.Length <= 4)
            {
                return 0;
            }
            else
            {
                int i = parts.Length - 4;
                string[] lastdisk = parts[i].Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); ;
                string disknum = Regex.Replace(lastdisk[1], @"[^0-9]+", "");
                int num = int.Parse(disknum);
                int totaldisk = num + 1;
                int a;
                for (a = 0; a < totaldisk; a++)
                {
                    string shell = String.Format("select disk {0} \r\ndetail disk \r\nexit", a);
                    Diskpart(shell);
                    WriteShow();
                    int issda = output.IndexOf("Linux");
                    if (issda != -1)
                    {
                        break;
                    }
                    ShellShow.Text = "";
                }
                if (a == totaldisk)
                {
                    return 0;
                }
                else
                {
                    ShellShow.Text = "";
                    return a;
                }
            }
        }

        public string Setletter(int Disknum, int Partnum)//������̷�
        {
            string letter = "C";
            int i;
            for (i = 65; i <= 90; i++)
            {
                char c = (char)i;
                letter = c.ToString();
                string shell = String.Format("select disk {0} \r\nselect part {1} \r\nassign letter={2} \r\nexit", Disknum, Partnum, letter);
                Diskpart(shell);
                WriteShow();
                int succ1 = output.IndexOf("�ɹ��ط������������Ż�װ�ص�");
                int succ2 = output.IndexOf("successfully assigned the drive letter or mount point");
                ShellShow.Text = "";
                if (succ1 != -1 || succ2 != -1)
                {
                    break;
                }
            }
            if (i == 91)
            {
                letter = "C";
            }
            return letter;
        }

        public bool Formatdisk(int Disknum, int Partnum, string Filesystem)//��ʽ������
        {
            string shell = String.Format("select disk {0} \r\nselect part {1} \r\nformat quick fs={2} \r\nexit", Disknum, Partnum, Filesystem);
            Diskpart(shell);
            WriteShow();
            ShellShow.Text = "";
            if (output.IndexOf("�ɹ�") != -1 || output.IndexOf("successfully") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Setid(bool Id, int Disknum, int Partnum)
        {
            string id;
            if (Id)
            {
                id = "ebd0a0a2-b9e5-4433-87c0-68b6b72699c7";//��Ҫ
            }
            else
            {
                id = "c12a7328-f81f-11d2-ba4b-00a0c93ec93b";//ϵͳ
            }
            string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Disknum);
            Diskpart(shell);
            WriteShow();
            shell = String.Format("select disk {0} \r\nselect part {1} \r\nset id='{2}' override\r\nexit", Disknum, Partnum, id);
            Diskpart(shell);
            WriteShow();
            if (output.IndexOf("�ɹ�") != -1 || output.IndexOf("successfully") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //�����豸״��
        string shell = "";//ADB����
        string part = "";//��������
        string sdxx = "";//sdx����
        string partnum = "";//�������
        int succ = 0;//һ������1��Ϊ��������
        string exepath = System.IO.Directory.GetCurrentDirectory();//��ȡ��������·��

        private async void MindowsInstallShown()
        {
            Mindows.Disdevice();//���ֻ���

            //����ʼ
            if (Global.devcfg)
            {
                ShowText.Text = "����ˢ��Devcfg";
                Fastboot(@"flash devcfg_ab data\mindows\img\devcfg.img");
            }
            if (Global.vbmeta)
            {
                ShowText.Text = "���ڹر�AVBУ��";
                Fastboot(@"flash vbmeta bin/vbmeta.img");
            }
            if (Global.bootrec)
            {
                string active = await ADBHelper.Fastboot("getvar current-slot");
                if (active.IndexOf("current-slot: b") != -1)
                {
                    Global.boot = "boot_b";
                }
                ShowText.Text = "������ʱ����Recovery";
                Fastboot(@"boot data\mindows\img\recovery.img");
            }
            else
            {
                ShowText.Text = "����ˢ�벢����Recovery";
                Fastboot(@"flash recovery data\mindows\img\recovery.img");
                Fastboot("oem reboot-recovery");
            }
            for (int i; ;)
            {     
                string AdbDevices = await ADBHelper.ADB("devices");
                i = AdbDevices.IndexOf("recovery");
                if (i != -1)
                    break;
            }
            _ = ADBHelper.ADB("shell twrp unmount data");
            ShowText.Text = "���ڱ�����Ҫ����";
            Mindows.GetPartTable();
            part = Global.boot;
            Mindows.Backup(part);
            part = "fsc";
            Mindows.Backup(part);
            part = "fsg";
            Mindows.Backup(part);
            part = "modem";
            Mindows.Backup(part);
            part = "modemst1";
            Mindows.Backup(part);
            part = "modemst2";
            Mindows.Backup(part);
            ShowText.Text = "��������parted�����ݷ�����";
            _ = ADBHelper.ADB("push bin/parted /tmp/");
            _ = ADBHelper.ADB("shell chmod +x /tmp/parted");
            string parttable = await ADBHelper.ADB("shell /tmp/parted /dev/block/sda print");
            ShellShow.Text = parttable;
            Mindows.Write("backup/parts.txt", parttable);
            if (Mindows.Isdatalast(parttable))
            {
                if (Global.removelimit)
                {
                    ShowText.Text = "���ڽ��������������";
                    _ = ADBHelper.ADB("push bin/sgdisk /tmp/");
                    _ = ADBHelper.ADB("shell chmod +x /tmp/sgdisk");
                    string limit = await ADBHelper.ADB("shell /tmp/sgdisk --resize-table=128 /dev/block/sda");
                    if (limit.IndexOf("completed successfully") == -1)
                    {
                        succ = 1;//��ֵһ������1��Ϊ����
                    }
                    else
                    {
                        if (Global.bootrec)
                        {
                            _ = ADBHelper.ADB("reboot bootloader");
                            for (; ; )
                            {
                                string FastbootDevices = await ADBHelper.Fastboot("devices");
                                if (FastbootDevices != "")
                                    break;
                            }
                            Fastboot(@"boot data\mindows\img\recovery.img");
                        }
                        else
                        {
                            _ = ADBHelper.ADB("reboot recovery");
                        }
                        for (int i; ;)
                        {
                            string AdbDevices = await ADBHelper.ADB("devices");
                            i = AdbDevices.IndexOf("recovery");
                            if (i != -1)
                                break;
                        }
                    }
                }
                if (succ == 0)
                {
                    _ = ADBHelper.ADB("shell twrp unmount data");
                    part = "userdata";
                    int datano = Mindows.Onlynum(Mindows.Partno(parttable, part));
                    Global.datastartunit = Mindows.Unit(Mindows.Partstart(parttable, part));
                    Global.dataendunit = Mindows.Unit(Mindows.Partend(parttable, part));
                    Global.datasizeunit = Mindows.Unit(Mindows.Partsize(parttable, part));
                    Global.datastart = Mindows.Onlynum(Mindows.Partstart(parttable, part));
                    string datastart2 = Mindows.Partstart(parttable, part);
                    Global.dataend = Mindows.Onlynum(Mindows.Partend(parttable, part));
                    Global.datasize = Mindows.Onlynum(Mindows.Partsize(parttable, part));
                    ShowText.Text = "����ΪWindows����";
                    SetWinPart setwinpart = new()
                    {
                        Title = "�趨Windows����"
                    };
                    setwinpart.Activate();
                    if (Global.winsize != 0)
                    {
                        _ = ADBHelper.ADB("shell twrp unmount data");
                        _ = ADBHelper.ADB("push bin/parted /tmp/");
                        _ = ADBHelper.ADB("shell chmod +x /tmp/parted");
                        shell = string.Format("shell /tmp/parted /dev/block/sda rm {0}", datano);
                        _ = ADBHelper.ADB(shell);
                        int newdataend = Global.dataend - Global.winsize - Global.sharepartsize;
                        shell = string.Format("shell /tmp/parted /dev/block/sda mkpart userdata ext4 {0} {1}GB", datastart2, newdataend);
                        _ = ADBHelper.ADB(shell);
                        int espstart = newdataend;
                        double espend = newdataend + 0.3;
                        shell = string.Format("shell /tmp/parted /dev/block/sda mkpart esp fat32 {0}GB {1}GB", espstart, espend);
                        _ = ADBHelper.ADB(shell);
                        double winstart = espend;
                        int winend = Global.dataend - Global.sharepartsize;
                        shell = string.Format("shell /tmp/parted /dev/block/sda mkpart win ntfs {0}GB {1}GB", winstart, winend);
                        _ = ADBHelper.ADB(shell);
                        if (Global.sharepartsize != 0 && Global.mksharepart)
                        {
                            int sharestar = winend;
                            int shareend = Global.dataend;
                            shell = string.Format("shell /tmp/parted /dev/block/sda mkpart sharedspace ext4 {0}GB {1}GB", sharestar, shareend);
                            _ = ADBHelper.ADB(shell);
                        }
                        parttable = await ADBHelper.ADB("shell /tmp/parted /dev/block/sda print");
                        if (parttable.IndexOf("esp") != -1)
                        {
                            part = "esp";
                            int espno = Mindows.Onlynum(Mindows.Partno(parttable, part));
                            shell = string.Format("shell /tmp/parted /dev/block/sda set {0} esp on", espno);
                            _ = ADBHelper.ADB(shell);
                        }
                        parttable = await ADBHelper.ADB("shell /tmp/parted /dev/block/sda print");
                        if (parttable.IndexOf("userdata") == -1 || parttable.IndexOf("win") == -1 || parttable.IndexOf("esp") == -1)
                        {
                            succ = 1;
                        }
                        ShellShow.Text = parttable;
                        if (succ == 0)
                        {
                            ShowText.Text = "��������Recovery����ʽ���½�����";
                            if (Global.bootrec)
                            {
                                _ = ADBHelper.ADB("reboot bootloader");
                                for (; ; )
                                {
                                    string FastbootDevices = await ADBHelper.Fastboot("devices");
                                    if (FastbootDevices != "")
                                        break;
                                }
                                Fastboot(@"boot data\mindows\img\recovery.img");
                            }
                            else
                            {
                                _ = ADBHelper.ADB("reboot recovery");
                            }
                            for (int i; ;)
                            {
                                string FastbootDevices = await ADBHelper.Fastboot("devices");
                                i = FastbootDevices.IndexOf("recovery");
                                if (i != -1)
                                    break;
                            }
                            _ = ADBHelper.ADB("shell twrp unmount data");
                            Mindows.GetPartTable();
                            part = "esp";
                            sdxx = Mindows.FindDisk(part);
                            if (sdxx != "")
                            {
                                partnum = Mindows.Partno(Mindows.FindPart(part), part);
                                shell = string.Format("shell mkfs.fat -F32 -s1 /dev/block/{0}{1}", sdxx, partnum);
                                ADB(shell);
                            }
                            _ = ADBHelper.ADB("shell twrp unmount data");
                            part = "userdata";
                            sdxx = Mindows.FindDisk(part);
                            if (sdxx != "")
                            {
                                partnum = Mindows.Partno(Mindows.FindPart(part), part);
                                shell = string.Format("shell mke2fs -t ext4 /dev/block/{0}{1}", sdxx, partnum);
                                ADB(shell);
                            }
                            if (Global.mksharepart)
                            {
                                part = "sharedspace";
                                sdxx = Mindows.FindDisk(part);
                                if (sdxx != "")
                                {
                                    partnum = Mindows.Partno(Mindows.FindPart(part), part);
                                    shell = string.Format("shell mkexfatfs -n exfat /dev/block/{0}{1}", sdxx, partnum);
                                    ADB(shell);
                                }
                            }
                            ShowText.Text = "���ڽ��������ģʽ";
                            TipsText.Text = "��ȴ��ϳ�ʱ���޷�Ӧ�뽫�豸�ֶ�����Fastbootģʽ��";
                            Thread.Sleep(2000);
                            await ADBHelper.ADB("reboot bootloader");
                            for (; ; )
                            {
                                string FastbootDevices = await ADBHelper.Fastboot("devices");
                                if (FastbootDevices != "")
                                    break;
                            }
                            Fastboot(@"flash boot data\mindows\img\automass.img");
                            Fastboot("reboot");
                            ShellShow.Text = "";
                            ShowText.Text = "���ڲ���Sda����";
                            TipsText.Text = "";
                            int sdanum;
                            if (Global.issda)
                            {
                                for (sdanum = 0; ;)
                                {
                                    sdanum = Findsda();
                                    if (sdanum != 0)
                                        break;
                                }
                            }
                            else
                            {
                                for (sdanum = 0; ;)
                                {
                                    sdanum = FindLinux();
                                    if (sdanum != 0)
                                        break;
                                }
                            }
                            ShowText.Text = "���ڲ���Windows����";
                            shell = String.Format("select disk {0} \r\nlist part \r\nexit", sdanum);
                            Diskpart(shell);
                            WriteShow();
                            bool diskstate = false;
                            if (output.IndexOf("ϵͳ") != -1 && output.IndexOf("��Ҫ") != -1)
                            {
                                diskstate = true;
                            }
                            else if (output.IndexOf("System") != -1 && output.IndexOf("Primary") != -1)
                            {
                                diskstate = true;
                            }
                            if (diskstate)
                            {
                                bool setid = true;
                                int winsum = Mindows.FindWin(output);
                                int uefisum = Mindows.FindUEFI(output);
                                ShellShow.Text = "";
                                ShowText.Text = "���ڷ�����̷�";
                                setid = Setid(true, sdanum, uefisum);
                                string winletter = Setletter(sdanum, winsum);
                                string uefiletter = Setletter(sdanum, uefisum);
                                if (setid && winletter != "C" && uefiletter != "C")
                                {
                                    ShowText.Text = "���ڸ�ʽ��Windows����";
                                    Formatdisk(sdanum, winsum, "ntfs");
                                    Formatdisk(sdanum, uefisum, "fat32");
                                    ShowText.Text = "ѡ��Windows����";
                                    ChooseIso choicewin = new();
                                    choicewin.Activate();
                                    if (Global.wimpath != "")
                                    {
                                        ShowText.Text = "���ڰ�װWindows";
                                        string drvpath = string.Format(@"{0}\data\mindows\driver", exepath);
                                        shell = string.Format("wim {0} {1}", winletter, Global.wimpath);
                                        Dism(shell);
                                        if (Global.havedrv)
                                        {
                                            shell = string.Format("drv {0} {1}", winletter, drvpath);
                                            Dism(shell);
                                        }
                                        ShellShow.Text = "";
                                        shell = string.Format(@"{0}:\Windows /s {1}: /f UEFI /l zh-cn", winletter, uefiletter);
                                        Bcdboot(shell);
                                        WriteShow();
                                        if (output.IndexOf("successfully") != -1)
                                        {
                                            shell = string.Format(@"/store {0}:\efi\microsoft\boot\bcd /set {{Default}} testsigning on", uefiletter);
                                            Bcdedit(shell);
                                            shell = string.Format(@"/store {0}:\efi\microsoft\boot\bcd /set {{Default}} nointegritychecks on", uefiletter);
                                            Bcdedit(shell);
                                            setid = Setid(false, sdanum, uefisum);
                                            ShowText.Text = "���ڵȴ�����Fastboot";
                                            TipsText.Text = "������Դ���������������豸����Fastootģʽ";
                                            await ShowDialog("Windows��װ����뽫�豸������Fastbootģʽ!");
                                            for (; ; )
                                            {
                                                string FastbootDevice = await ADBHelper.Fastboot("devices");
                                                if (FastbootDevice != "")
                                                    break;
                                            }
                                            ShowText.Text = "����ˢ��UEFI��������Windows";
                                            TipsText.Text = "";
                                            shell = string.Format(@"flash {0} data\mindows\img\uefi.img", Global.boot);
                                            Fastboot(shell);
                                            Fastboot("reboot");
                                            ShowText.Text = "��ɣ�";
                                        }
                                        else
                                        {
                                            await ShowDialog("������������ʧ�ܣ�");
                                            ShowText.Text = "������������ʧ�ܣ�";
                                            TipsText.Text = "��ȷ����ǰWindows�汾�����ͷ�Ŀ��汾Windows����";
                                        }
                                    }
                                    else
                                    {
                                        await ShowDialog("δѡ��Windows���񣬳������");
                                        ShowText.Text = "δѡ��Windows���񣬳������";
                                        this.Close();
                                    }
                                }
                                else
                                {
                                    await ShowDialog("������̷��Ż�����IDʧ�ܣ�");
                                    ShowText.Text = "������̷��Ż�����IDʧ�ܣ�";
                                    TipsText.Text = "�������������ԵĴ���������ȷ��ϵͳΪWin10����߰汾";
                                }
                            }
                            else
                            {
                                await ShowDialog("δ���ҵ�Windows��UEFI������");
                                ShowText.Text = "δ���ҵ�Windows��UEFI������";
                            }
                        }
                        else
                        {
                            await ShowDialog("�������ִ������������");
                            ShowText.Text = "�������ִ������������";
                        }
                    }
                    else
                    {
                        await ShowDialog("��δ�趨Windows������С���������");
                        ShowText.Text = "��δ�趨Windows������С���������";
                        this.Close();
                    }
                }
                else
                {
                    await ShowDialog("�����������ʧ�ܣ�");
                    ShowText.Text = "�����������ʧ�ܣ�";
                }
            }
            else
            {
                await ShowDialog("Data�����������һ�����������������");
                ShowText.Text = "Data�����������һ�����������������";
                TipsText.Text = "����ˢ�����볢�Իָ�����������ԣ�";
            }
        }
    }
}
