using System;
using System.Diagnostics;
using System.IO;

namespace Service
{

    public static class PathMgr
    {
        private static readonly string ConfigDir = "config/";
        private static readonly string _UserConfigDir = "userconfig/";
        private static readonly string ConfigFile = "FlashProgram.config";
        private static readonly string _sessionFile = "usersesstion.sess";
        private static readonly string ConsoleHelpFile = "console_help.txt";
        public static readonly string ConfigFileExt = "config";

        public static string DefaultUserConfigFile
        {
            get => Path.Combine(UserConfigDir, Session.DefaultConfig);
        }

        public static string SessionFile => Path.Combine(GetAppConfigDir(), _sessionFile);

        public static string UserConfigDir
        {
            get
            {
                string path = Path.Combine(GetAppRuntimeDir(), _UserConfigDir);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public static string GetAppRuntimeDir()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + Path.DirectorySeparatorChar.ToString();
        }

        public static string GetAppConfigDir()
        {
            string path = GetAppRuntimeDir() + ConfigDir;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static string GetAppConfigFile() => GetAppConfigDir() + ConfigFile;

        public static string GetConsoleHelpFile() => GetAppConfigDir() + ConsoleHelpFile;

        public static string MakeRelativeToRuntimeDir(string path)
        {
            Uri uri = new Uri(GetAppRuntimeDir());
            try
            {
                return Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString());
            }
            catch (Exception)
            {
                return path;
            }
        }
    }
}
