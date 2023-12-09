using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Toolbox
{
    public class ADBHelper
    {
        public static async Task<string> ADB(string adb)
        {
            string adbPath = @"lib\adb.exe";
            string output = "";
            using (Process process = new())
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = adbPath,
                    Arguments = adb,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                process.StartInfo = startInfo;
                process.Start();
                output = await process.StandardOutput.ReadToEndAsync();
                string errorOutput = await process.StandardError.ReadToEndAsync();
                if (string.IsNullOrEmpty(output)) output = errorOutput;
                process.WaitForExit();
            }
            return output;
        }


        public static async Task<string> Fastboot(string fb)
        {
            string cmd = @"lib\fastboot.exe";
            ProcessStartInfo fastboot = new(cmd, fb)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            Process f = Process.Start(fastboot);
            StreamReader readererror = f.StandardError;
            StreamReader reader = f.StandardOutput;
            string output = await readererror.ReadToEndAsync();
            if (output == "")
            {
                output = await reader.ReadToEndAsync();
            }
            f.Close();
            return output;
        }
    }
}
