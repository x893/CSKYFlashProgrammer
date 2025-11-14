using System;
using System.Diagnostics;

namespace CskyFlashProgramer.UI
{

    public class CalcCRC
    {
        public static string GetCRCValue(string path, CRCType type)
        {
            if (path == "")
                return "";
            string str = "NULL";
            Process process = null;
            try
            {
                process = new Process();
                switch (type)
                {
                    case CRCType.CRC16:
                        process.StartInfo.FileName = "crc16.exe";
                        break;
                    case CRCType.CRC32:
                        process.StartInfo.FileName = "crc32.exe";
                        break;
                }
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                process.StartInfo.Arguments = $"\"{path}\"";
                process.Start();
                str = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception)
            {
            }
            finally
            {
                process?.Close();
            }
            string crcValue = str.TrimEnd();
            if (!crcValue.StartsWith("0x"))
                crcValue = "NULL";
            return crcValue;
        }
    }
}