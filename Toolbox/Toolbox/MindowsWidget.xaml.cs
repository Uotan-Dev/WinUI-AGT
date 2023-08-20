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
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using WinRT.Interop;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Toolbox
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MindowsWidget : Window
    {
        public MindowsWidget()
        {
            this.InitializeComponent();
            Moreability();
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

        readonly string exepath = System.IO.Directory.GetCurrentDirectory();//��ȡ��������·��
        string output;//���
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

        public void Reg(string fb)
        {
            string cmd = @"reg.exe";
            ProcessStartInfo fastboot = null;
            fastboot = new ProcessStartInfo(cmd, fb)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process f = Process.Start(fastboot);
            f.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            f.BeginOutputReadLine();
            f.BeginErrorReadLine();
            f.WaitForExit();
            f.Close();
        }

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
                id = "ebd0a0a2-b9e5-4433-87c0-68b6b72699c7";
            }
            else
            {
                id = "c12a7328-f81f-11d2-ba4b-00a0c93ec93b";
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

        public async void Moreability()
        {
            if (Global.moreability == "formatesp")
            {
                WarnText.Visibility = Visibility.Collapsed;
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                ConfirmButton.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                Mindows.Disdevice();//���ֻ���
                if (Global.sdanum == 0)
                {
                    ShowText.Text = "���ڲ���Sda����";
                    if (Global.issda)
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = Findsda();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = FindLinux();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                }
                ShowText.Text = "���ڸ�ʽ��UEFI����";
                string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                Diskpart(shell);
                WriteShow();
                if (output.IndexOf("ϵͳ") != -1 || output.IndexOf("System") != -1)
                {
                    int uefisum = Mindows.FindUEFI(output);
                    ShellShow.Text = "";
                    Setid(true, Global.sdanum, uefisum);
                    Formatdisk(Global.sdanum, uefisum, "fat32");
                    Setid(false, Global.sdanum, uefisum);
                    ShowText.Text = "��ɣ�";
                    _ = _ = ShowDialog("��ʽ����ɣ�");
                }
                else
                {
                    _ = _ = ShowDialog("δ�ҵ�UEFI������");
                }
            }
            else if (Global.moreability == "formatwin")
            {
                WarnText.Visibility = Visibility.Collapsed;
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                ConfirmButton.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                Mindows.Disdevice();//���ֻ���
                if (Global.sdanum == 0)
                {
                    ShowText.Text = "���ڲ���Sda����";
                    if (Global.issda)
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = Findsda();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = FindLinux();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                }
                ShowText.Text = "���ڸ�ʽ��Windows����";
                string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                Diskpart(shell);
                WriteShow();
                if (output.IndexOf("��Ҫ") != -1 || output.IndexOf("Primary") != -1)
                {
                    int winsum = Mindows.FindWin(output);
                    ShellShow.Text = "";
                    Formatdisk(Global.sdanum, winsum, "ntfs");
                    ShowText.Text = "��ɣ�";
                    _ = _ = ShowDialog("��ʽ����ɣ�");
                }
                else
                {
                    _ = _ = ShowDialog("δ�ҵ�UEFI������");
                }
            }
            else if (Global.moreability == "fixesp")
            {
                WarnText.Visibility = Visibility.Collapsed;
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                ConfirmButton.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                Mindows.Disdevice();//���ֻ���
                if (Global.sdanum == 0)
                {
                    ShowText.Text = "���ڲ���Sda����";
                    if (Global.issda)
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = Findsda();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = FindLinux();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                }
                ShowText.Text = "�����޸���������";
                if (Global.winletter == "C" || Global.uefisum == 0)
                {
                    string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
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
                        int winsum = Mindows.FindWin(output);
                        Global.uefisum = Mindows.FindUEFI(output);
                        ShellShow.Text = "";
                        Global.winletter = Setletter(Global.sdanum, winsum);
                    }
                    else
                    {
                        _ = _ = ShowDialog("δ�ҵ�Windows��UEFI������");
                    }
                }
                if (Global.winletter != "C" && Global.uefisum != 0)
                {
                    Setid(true, Global.sdanum, Global.uefisum);
                    string uefiletter = Setletter(Global.sdanum, Global.uefisum);
                    if (uefiletter != "C")
                    {
                        ShellShow.Text = "";
                        string shell = String.Format(@"{0}:\Windows /s {1}: /f UEFI /l zh-cn", Global.winletter, uefiletter);
                        Bcdboot(shell);
                        WriteShow();
                        Setid(false, Global.sdanum, Global.uefisum);
                        if (output.IndexOf("successfully") != -1)
                        {
                            _ = _ = ShowDialog("�޸��ɹ���");
                        }
                        else
                        {
                            _ = _ = ShowDialog("�޸�ʧ�ܣ�");
                        }
                    }
                    else
                    {
                        _ = _ = ShowDialog("���̷��������⣬����ʧ�ܣ�");
                    }
                }
                else
                {
                    _ = _ = ShowDialog("���̷��������⣬����ʧ�ܣ�");
                }
            }
            else if (Global.moreability == "disdsv")
            {
                WarnText.Visibility = Visibility.Collapsed;
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                ConfirmButton.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                Mindows.Disdevice();//���ֻ���
                if (Global.sdanum == 0)
                {
                    ShowText.Text = "���ڲ���Sda����";
                    if (Global.issda)
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = Findsda();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = FindLinux();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                }
                ShowText.Text = "���ڽ�������ǩ��";
                if (Global.uefisum == 0)
                {
                    string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                    Diskpart(shell);
                    WriteShow();
                    if (output.IndexOf("ϵͳ") != -1 || output.IndexOf("System") != -1)
                    {
                        Global.uefisum = Mindows.FindUEFI(output);
                    }
                    else
                    {
                        _ = _ = ShowDialog("δ�ҵ�UEFI������");
                    }
                }
                if (Global.uefisum != 0)
                {
                    Setid(true, Global.sdanum, Global.uefisum);
                    string uefiletter = Setletter(Global.sdanum, Global.uefisum);
                    if (uefiletter != "C")
                    {
                        string shell = String.Format(@"/store {0}:\efi\microsoft\boot\bcd /set {{Default}} testsigning on", uefiletter);
                        Bcdedit(shell);
                        shell = String.Format(@"/store {0}:\efi\microsoft\boot\bcd /set {{Default}} nointegritychecks on", uefiletter);
                        Bcdedit(shell);
                        Setid(false, Global.sdanum, Global.uefisum);
                        ShowText.Text = "��ɣ�";
                    }
                    else
                    {
                        _ = _ = ShowDialog("���̷��������⣬����ʧ�ܣ�");
                    }
                }
                else
                {
                    _ = _ = ShowDialog("���̷��������⣬����ʧ�ܣ�");
                }
            }
            else if (Global.moreability == "installdrive")
            {
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                string drvpath1 = String.Format(@"{0}\data\mindows\driver", exepath);
                if (Directory.Exists(drvpath1))
                {
                    WarnText.Text = "��ǰ·��ΪĬ������·�������������ڰ�װWindowsʱ��װ";
                    DrvPath.Text = drvpath1;
                }
                else
                {
                    WarnText.Text = "δ�ҵ�Ĭ����������ָ��·����";
                }
            }
            else if (Global.moreability == "repart")
            {
                WarnText.Visibility = Visibility.Collapsed;
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                ConfirmButton.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                Mindows.Disdevice();//���ֻ���
                ShowText.Text = "���ڼ�������";
                _ = await ADBHelper.ADB("shell twrp unmount data");
                _ = await ADBHelper.ADB("push bin/parted /tmp/");
                _ = await ADBHelper.ADB("shell chmod +x /tmp/parted");
                string parttable = await ADBHelper.ADB("shell /tmp/parted /dev/block/sda print");
                ShellShow.Text = parttable;
                string shell = "";//ADB����
                string part = "";//��������
                string sdxx = "";//sdx����
                string partnum = "";//�������
                if (parttable.IndexOf("userdata") != -1 && parttable.IndexOf("win") != -1 && parttable.IndexOf("esp") != -1)
                {
                    part = "userdata";
                    int datano = Mindows.Onlynum(Mindows.Partno(parttable, part));
                    string datastart = Mindows.Partstart(parttable, part);
                    part = "esp";
                    int espno = Mindows.Onlynum(Mindows.Partno(parttable, part));
                    part = "win";
                    int winno = Mindows.Onlynum(Mindows.Partno(parttable, part));
                    string dataend = Mindows.Endpartend(parttable);
                    if (parttable.IndexOf("sharedspace") != -1)
                    {
                        part = "sharedspace";
                        int sharedspaceno = Mindows.Onlynum(Mindows.Partno(parttable, part));
                        shell = string.Format("shell /tmp/parted /dev/block/sda rm {0}", sharedspaceno);
                        _ = ADBHelper.ADB(shell);
                    }
                    shell = string.Format("shell /tmp/parted /dev/block/sda rm {0}", espno);
                    _ = ADBHelper.ADB(shell);
                    shell = string.Format("shell /tmp/parted /dev/block/sda rm {0}", winno);
                    _ = ADBHelper.ADB(shell);
                    _ = await ADBHelper.ADB("shell twrp unmount data");
                    shell = string.Format("shell /tmp/parted /dev/block/sda rm {0}", datano);
                    _ = ADBHelper.ADB(shell);
                    shell = string.Format("shell /tmp/parted /dev/block/sda mkpart userdata ext4 {0} {1}", datastart, dataend);
                    _ = ADBHelper.ADB(shell);
                    parttable = await ADBHelper.ADB("shell /tmp/parted /dev/block/sda print");
                    ShellShow.Text = parttable;
                    if (parttable.IndexOf("userdata") != -1)
                    {
                        ShowText.Text = "��������Rec����ʽ��Data����";
                        if (Global.bootrec)
                        {
                            _ = await ADBHelper.ADB("reboot bootloader");
                            for (; ; )
                            {
                                if (await ADBHelper.Fastboot("devices") != "")
                                    break;
                            }
                            Fastboot(@"boot data\mindows\img\recovery.img");
                        }
                        else
                        {
                            _ = await ADBHelper.ADB("reboot recovery");
                        }
                        for (int i; ;)
                        {
                            string istring = await ADBHelper.ADB("devices");
                            i = istring.IndexOf("recovery");
                            if (i != -1)
                                break;
                        }
                        _ = await ADBHelper.ADB("shell twrp unmount data");
                        Mindows.GetPartTable();
                        _ = await ADBHelper.ADB("shell twrp unmount data");
                        part = "userdata";
                        sdxx = Mindows.FindDisk(part);
                        if (sdxx != "")
                        {
                            partnum = Mindows.Partno(Mindows.FindPart(part), part);
                            shell = string.Format("shell mke2fs -t ext4 /dev/block/{0}{1}", sdxx, partnum);
                            ADB(shell);
                        }
                        ShowText.Text = "�ָ���ɣ�";
                        _ = _ = ShowDialog("�ָ���ɣ�");
                    }
                    else
                    {
                        ShowText.Text = "�ָ��������ִ���";
                        _ = _ = ShowDialog("�ָ��������ִ���");
                    }
                }
                else
                {
                    ShowText.Text = "δ�ҵ���Ҫɾ���ķ�����";
                    _ = _ = ShowDialog("δ�ҵ���Ҫɾ���ķ�����");
                }
            }
            else if (Global.moreability == "installwin")
            {
                WarnText.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                ChooseDrv.Text = "ѡ���ļ�";
                ShowText.Text = "��ѡ��Ҫ��װ��Windows����";
            }
            else if (Global.moreability == "noweboobe")
            {
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                ConfirmButton.Content = "��ʼ";
                WarnText.Text = "��������Windows�����򵼵�������¼";
            }
            else if (Global.moreability == "usbmode")
            {
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                ConfirmButton.Content = "��ʼ";
                WarnText.Text = "�޸�USBģʽ���޸�ĳЩ�豸USB�޷����ӵ�����";
            }
            else if (Global.moreability == "oobeerror")
            {
                DrvPath.Visibility = Visibility.Collapsed;
                ChooseDrv.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                ConfirmButton.Content = "��ʼ";
                WarnText.Text = "���Խ��Windows�������ֵ����޷�����ϵͳ������";
            }
            else if (Global.moreability == "loadreg")
            {
                WarnText.Visibility = Visibility.Collapsed;
                Set1.Visibility = Visibility.Collapsed;
                Set3.Visibility = Visibility.Collapsed;
                Set6.Visibility = Visibility.Collapsed;
                ConfirmButton.Content = "����";
                ChooseDrv.Text = "ѡ���ļ�";
                ShowText.Text = "��ѡ��Ҫ���ص�ע���";
            }
            else
            {
                _ = _ = ShowDialog("δ�趨ִ�����ݣ�");
            }
        }

        // ����

        private async void ChooseDrvClick(object sender, EventArgs e)
        {
            if (Global.moreability == "installwin")
            {
                var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                nint windowHandle = WindowNative.GetWindowHandle(App.Window);
                InitializeWithWindow.Initialize(openPicker, windowHandle);
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add("*");
                var file = await openPicker.PickSingleFileAsync();
                if (file != null) DrvPath.Text = file.Path;
                else DrvPath.PlaceholderText = "������ȡ��";
            }
            else if (Global.moreability == "installdrive")
            {
                var folderPicker = new Windows.Storage.Pickers.FolderPicker();
                nint windowHandle = WindowNative.GetWindowHandle(App.Window);
                InitializeWithWindow.Initialize(folderPicker, windowHandle);
                folderPicker.ViewMode = PickerViewMode.Thumbnail;
                folderPicker.FileTypeFilter.Add("*");
                var folder = await folderPicker.PickSingleFolderAsync();
                if (folder != null) DrvPath.Text = folder.Path;
                else DrvPath.PlaceholderText = "������ȡ��";
            }
            else if (Global.moreability == "loadreg")
            {
                var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                nint windowHandle = WindowNative.GetWindowHandle(App.Window);
                InitializeWithWindow.Initialize(openPicker, windowHandle);
                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add("*");
                var file = await openPicker.PickSingleFileAsync();
                if (file != null) DrvPath.Text = file.Path;
                else DrvPath.PlaceholderText = "������ȡ��";
            }
            else
            {
                _ = _ = ShowDialog("δ�趨ִ�����ݣ�");
            }
        }


        private void installdrv_Click(object sender, EventArgs e)
        {
            if (Global.moreability == "installwin")
            {
                Mindows.Disdevice();//���ֻ���
                string wimpath = DrvPath.Text;
                if (wimpath != "")
                {
                    ConfirmButton.IsEnabled = false;
                    if (Global.sdanum == 0)
                    {
                        ShowText.Text = "���ڲ���Sda����";
                        if (Global.issda)
                        {
                            for (Global.sdanum = 0; ;)
                            {
                                Global.sdanum = Findsda();
                                if (Global.sdanum != 0)
                                    break;
                            }
                        }
                        else
                        {
                            for (Global.sdanum = 0; ;)
                            {
                                Global.sdanum = FindLinux();
                                if (Global.sdanum != 0)
                                    break;
                            }
                        }
                    }
                    ShowText.Text = "���ڲ���Windows����";
                    string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
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
                        int winsum = Mindows.FindWin(output);
                        Global.uefisum = Mindows.FindUEFI(output);
                        ShellShow.Text = "";
                        Global.winletter = Setletter(Global.sdanum, winsum);
                        Setid(true, Global.sdanum, Global.uefisum);
                        string uefiletter = Setletter(Global.sdanum, Global.uefisum);
                        if (Global.winletter != "C" && uefiletter != "C")
                        {
                            ShowText.Text = "���ڰ�װWindows";
                            shell = String.Format("wim {0} {1}", Global.winletter, wimpath);
                            Dism(shell);
                            ShellShow.Text = "";
                            shell = String.Format(@"{0}:\Windows /s {1}: /f UEFI /l zh-cn", Global.winletter, uefiletter);
                            Bcdboot(shell);
                            WriteShow();
                            Setid(false, Global.sdanum, Global.uefisum);
                            if (output.IndexOf("successfully") != -1)
                            {
                                ShowText.Text = "��ɣ�";
                            }
                            else
                            {
                                _ = ShowDialog("������������ʧ�ܣ�");
                                ShowText.Text = "������������ʧ�ܣ�";
                            }
                        }
                        else
                        {
                            _ = ShowDialog("���̷��������⣬����ʧ�ܣ�");
                        }
                    }
                    else
                    {
                        _ = ShowDialog("δ�ҵ�Windows��UEFI������");
                    }
                }
                else
                {
                    _ = ShowDialog("��ѡ��Windows����");
                }
                ConfirmButton.IsEnabled = true;
            }
            else if (Global.moreability == "installdrive")
            {
                Mindows.Disdevice();//���ֻ���
                string drvpath1 = DrvPath.Text;
                if (drvpath1 != "")
                {
                    ConfirmButton.IsEnabled = false;
                    if (Global.sdanum == 0)
                    {
                        ShowText.Text = "���ڲ���Sda����";
                        if (Global.issda)
                        {
                            for (Global.sdanum = 0; ;)
                            {
                                Global.sdanum = Findsda();
                                if (Global.sdanum != 0)
                                    break;
                            }
                        }
                        else
                        {
                            for (Global.sdanum = 0; ;)
                            {
                                Global.sdanum = FindLinux();
                                if (Global.sdanum != 0)
                                    break;
                            }
                        }
                    }
                    ShowText.Text = "���ڰ�װ��������";
                    if (Global.winletter == "C")
                    {
                        string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                        Diskpart(shell);
                        WriteShow();
                        if (output.IndexOf("��Ҫ") != -1 || output.IndexOf("Primary") != -1)
                        {
                            int winsum = Mindows.FindWin(output);
                            ShellShow.Text = "";
                            Global.winletter = Setletter(Global.sdanum, winsum);
                        }
                        else
                        {
                            _ = ShowDialog("δ�ҵ�Windows������");
                        }
                    }
                    if (Global.winletter != "C")
                    {
                        string shell = String.Format("drv {0} {1}", Global.winletter, drvpath1);
                        Dism(shell);
                        ShowText.Text = "��ɣ�";
                    }
                    else
                    {
                        _ = ShowDialog("���̷��������⣬����ʧ�ܣ�");
                    }
                }
                else
                {
                    _ = ShowDialog("��ѡ�������ļ�·����");
                }
                ConfirmButton.IsEnabled = true;
            }
            else if (Global.moreability == "noweboobe")
            {
                ConfirmButton.IsEnabled = false;
                Mindows.Disdevice();//���ֻ���
                if (Global.sdanum == 0)
                {
                    ShowText.Text = "���ڲ���Sda����";
                    if (Global.issda)
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = Findsda();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = FindLinux();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                }
                ShowText.Text = "���ڷ�����̷�";
                string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                Diskpart(shell);
                WriteShow();
                if (output.IndexOf("��Ҫ") != -1 || output.IndexOf("Primary") != -1)
                {
                    int winsum = Mindows.FindWin(output);
                    ShellShow.Text = "";
                    Global.winletter = Setletter(Global.sdanum, winsum);
                    ShowText.Text = "�����޸�ע���";
                    string patch = String.Format(@"load HKEY_LOCAL_MACHINE\Mindows {0}:\Windows\System32\config\SOFTWARE", Global.winletter);
                    Reg(patch);
                    if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                    {
                        ShellShow.Text = "";
                        RegistryHelper.EditRegedit(@"BypassNRO", 1, @"Microsoft\Windows\CurrentVersion\OOBE");
                        ShellShow.Text += RegistryHelper.GetRegistData(@"BypassNRO", @"Microsoft\Windows\CurrentVersion\OOBE");
                        if (ShellShow.Text.IndexOf("1") != -1)
                        {
                            int i;
                            for (i = 0; i < 1000; i++)
                            {
                                Reg(@"unload HKEY_LOCAL_MACHINE\Mindows");
                                if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                                {
                                    break;
                                }
                            }
                            if (i == 1000)
                            {
                                _ = ShowDialog("ж��ע���ʧ�ܣ�������������ֱ��ж�أ�");
                            }
                            else
                            {
                                ShowText.Text = "��ɣ�";
                                _ = ShowDialog("��ɣ�");
                            }
                        }
                        else
                        {
                            ShowText.Text = "δ�ҵ������ֱ�����";
                            RegistryHelper.WTRegedit(@"BypassNRO", 1, @"Microsoft\Windows\CurrentVersion\OOBE");
                            ShellShow.Text += RegistryHelper.GetRegistData(@"BypassNRO", @"Microsoft\Windows\CurrentVersion\OOBE");
                            if (ShellShow.Text.IndexOf("1") != -1)
                            {
                                int i;
                                for (i = 0; i < 1000; i++)
                                {
                                    Reg(@"unload HKEY_LOCAL_MACHINE\Mindows");
                                    if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                                    {
                                        break;
                                    }
                                }
                                if (i == 1000)
                                {
                                    _ = ShowDialog("ж��ע���ʧ�ܣ�������������ֱ��ж�أ�");
                                }
                                else
                                {
                                    ShowText.Text = "��ɣ�";
                                    _ = ShowDialog("��ɣ�");
                                }
                            }
                            else
                            {
                                _ = ShowDialog("�޸�ʧ�ܣ�");
                            }
                        }
                    }
                    else
                    {
                        _ = ShowDialog("����ע���ʧ�ܣ�");
                    }
                }
                else
                {
                    _ = ShowDialog("δ�ҵ�Windows������");
                }
                ConfirmButton.IsEnabled = true;
            }
            else if (Global.moreability == "usbmode")
            {
                int mode = 0;
                if (Set1.IsChecked == true)
                    mode = 1;
                if (Set3.IsChecked == true)
                    mode = 3;
                if (Set6.IsChecked == true)
                    mode = 6;
                if (mode != 0)
                {
                    ConfirmButton.IsEnabled = false;
                    Mindows.Disdevice();//���ֻ���
                    if (Global.sdanum == 0)
                    {
                        ShowText.Text = "���ڲ���Sda����";
                        if (Global.issda)
                        {
                            for (Global.sdanum = 0; ;)
                            {
                                Global.sdanum = Findsda();
                                if (Global.sdanum != 0)
                                    break;
                            }
                        }
                        else
                        {
                            for (Global.sdanum = 0; ;)
                            {
                                Global.sdanum = FindLinux();
                                if (Global.sdanum != 0)
                                    break;
                            }
                        }
                    }
                    ShowText.Text = "���ڷ�����̷�";
                    string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                    Diskpart(shell);
                    WriteShow();
                    if (output.IndexOf("��Ҫ") != -1 || output.IndexOf("Primary") != -1)
                    {
                        int winsum = Mindows.FindWin(output);
                        ShellShow.Text = "";
                        Global.winletter = Setletter(Global.sdanum, winsum);
                        ShowText.Text = "�����޸�ע���";
                        string patch = String.Format(@"load HKEY_LOCAL_MACHINE\Mindows {0}:\Windows\System32\config\SYSTEM", Global.winletter);
                        Reg(patch);
                        if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                        {
                            ShellShow.Text = "";
                            RegistryHelper.EditRegedit(@"OsDefaultRoleSwitchMode", mode, @"ControlSet001\Control\USB");
                            ShellShow.Text += RegistryHelper.GetRegistData(@"OsDefaultRoleSwitchMode", @"ControlSet001\Control\USB");
                            if (ShellShow.Text.IndexOf(mode.ToString()) != -1)
                            {
                                int i;
                                for (i = 0; i < 1000; i++)
                                {
                                    Reg(@"unload HKEY_LOCAL_MACHINE\Mindows");
                                    if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                                    {
                                        break;
                                    }
                                }
                                if (i == 1000)
                                {
                                    _ = ShowDialog("ж��ע���ʧ�ܣ�������������ֱ��ж�أ�");
                                }
                                else
                                {
                                    ShowText.Text = "��ɣ�";
                                    _ = ShowDialog("��ɣ�");
                                }
                            }
                            else
                            {
                                _ = ShowDialog("�޸�ʧ�ܣ�");
                            }
                        }
                        else
                        {
                            _ = ShowDialog("����ע���ʧ�ܣ�");
                        }
                    }
                    else
                    {
                        _ = ShowDialog("δ�ҵ�Windows������");
                    }
                }
                else
                {
                    _ = ShowDialog("��ѡ��ҪΪUSBģʽ���õ�ֵ��");
                }
                ConfirmButton.IsEnabled = true;
            }
            else if (Global.moreability == "oobeerror")
            {
                ConfirmButton.IsEnabled = false;
                Mindows.Disdevice();//���ֻ���
                if (Global.sdanum == 0)
                {
                    ShowText.Text = "���ڲ���Sda����";
                    if (Global.issda)
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = Findsda();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                    else
                    {
                        for (Global.sdanum = 0; ;)
                        {
                            Global.sdanum = FindLinux();
                            if (Global.sdanum != 0)
                                break;
                        }
                    }
                }
                ShowText.Text = "���ڷ�����̷�";
                string shell = String.Format("select disk {0} \r\nlist part \r\nexit", Global.sdanum);
                Diskpart(shell);
                WriteShow();
                if (output.IndexOf("��Ҫ") != -1 || output.IndexOf("Primary") != -1)
                {
                    int winsum = Mindows.FindWin(output);
                    ShellShow.Text = "";
                    Global.winletter = Setletter(Global.sdanum, winsum);
                    ShowText.Text = "�����޸�ע���";
                    string patch = String.Format(@"load HKEY_LOCAL_MACHINE\Mindows {0}:\Windows\System32\config\SYSTEM", Global.winletter);
                    Reg(patch);
                    if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                    {
                        ShellShow.Text = "";
                        bool reg = false;
                        reg = RegistryHelper.EditRegedit(@"OOBEInProgress", 0, @"Setup");
                        reg = RegistryHelper.EditRegedit(@"Respecialize", 0, @"Setup");
                        reg = RegistryHelper.EditRegedit(@"RestartSetup", 0, @"Setup");
                        reg = RegistryHelper.EditRegedit(@"SetupPhase", 0, @"Setup");
                        reg = RegistryHelper.EditRegedit(@"SetupSupported", 0, @"Setup");
                        reg = RegistryHelper.EditRegedit(@"SetupType", 0, @"Setup");
                        reg = RegistryHelper.EditRegedit(@"SystemSetupInProgress", 0, @"Setup");
                        if (reg)
                        {
                            ShowText.Text = "����ж��ע���";
                            int j;
                            for (j = 0; j < 1000; j++)
                            {
                                Reg(@"unload HKEY_LOCAL_MACHINE\Mindows");
                                if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                                {
                                    break;
                                }
                            }
                            if (j == 1000)
                            {
                                _ = ShowDialog("ж��ע���ʧ�ܣ�������������ֱ��ж�أ�");
                            }
                            else
                            {
                                ShowText.Text = "�����޸�ע���";
                                patch = String.Format(@"load HKEY_LOCAL_MACHINE\Mindows {0}:\Windows\System32\config\SAM", Global.winletter);
                                Reg(patch);
                                if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                                {
                                    ShellShow.Text = "";
                                    byte[] b = Mindows.Object2Bytes(RegistryHelper.GetRegistData(@"F", @"SAM\Domains\Account\Users\000001F4"));
                                    if (b[83].ToString("X2") == "11" || b[83].ToString("X2") == "10")
                                    {
                                        byte[] c = new byte[80];
                                        for (int i = 27; i <= 106; i++)
                                        {
                                            c[i - 27] = b[i];
                                        }
                                        c[56] = 16;
                                        RegistryHelper.EditRegedit(@"F", c, @"SAM\Domains\Account\Users\000001F4");
                                        byte[] a = Mindows.Object2Bytes(RegistryHelper.GetRegistData(@"F", @"SAM\Domains\Account\Users\000001F4"));
                                        if (a[83].ToString("X2") == "10")
                                        {
                                            ShowText.Text = "����ж��ע���";
                                            int i;
                                            for (i = 0; i < 1000; i++)
                                            {
                                                Reg(@"unload HKEY_LOCAL_MACHINE\Mindows");
                                                if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                                                {
                                                    break;
                                                }
                                            }
                                            if (i == 1000)
                                            {
                                                _ = ShowDialog("ж��ע���ʧ�ܣ�������������ֱ��ж�أ�");
                                            }
                                            else
                                            {
                                                ShowText.Text = "��ɣ�";
                                                _ = ShowDialog("��ɣ�");
                                            }
                                        }
                                        else
                                        {
                                            _ = ShowDialog("�޸�ʧ�ܣ�");
                                        }
                                    }
                                    else
                                    {
                                        _ = ShowDialog("��ǰ�û���ֵ��������ע���");
                                    }
                                }
                                else
                                {
                                    _ = ShowDialog("����ע���ʧ�ܣ�");
                                }
                            }
                        }
                        else
                        {
                            _ = ShowDialog("�޸�ʧ�ܣ�");
                        }
                    }
                    else
                    {
                        _ = ShowDialog("����ע���ʧ�ܣ�");
                    }
                }
                else
                {
                    _ = ShowDialog("δ�ҵ�Windows������");
                }
                ConfirmButton.IsEnabled = true;
            }
            else if (Global.moreability == "loadreg")
            {
                string regpath = DrvPath.Text;
                if (regpath != "")
                {
                    ConfirmButton.IsEnabled = false;
                    ShowText.Text = "���ڹ���ע���";
                    string patch = String.Format(@"load HKEY_LOCAL_MACHINE\Mindows {0}", regpath);
                    Reg(patch);
                    if (ShellShow.Text.IndexOf("�����ɹ����") != -1 || ShellShow.Text.IndexOf("operation completed successfully") != -1)
                    {
                        ShellShow.Text = @"�ѽ�ע����������HKEY_LOCAL_MACHINE\Mindows��";
                        Mindows.NSudoLC(@"-U:S -P:E -M:S regedit.exe");
                        ShowText.Text = "��ɣ�";
                        _ = ShowDialog("��ɣ�");
                    }
                    else
                    {
                        _ = ShowDialog("����ע���ʧ�ܣ�");
                    }
                    ConfirmButton.IsEnabled = true;
                }
                else
                {
                    _ = ShowDialog("��ѡ����Ҫ���ص�ע���");
                }
            }
            else
            {
                _ = ShowDialog("δ�趨ִ�����ݣ�");
            }
        }

    }
}
