using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Service
{

    public class AppConfigMgr
    {
        private static AppConfigMgr ms_instance;

        public AppConfig AppConfig { set; get; }

        public static AppConfigMgr Instance
        {
            get
            {
                if (ms_instance == null)
                    ms_instance = new AppConfigMgr();
                return ms_instance;
            }
        }

        public static AppConfigMgr Get() => Instance;

        private void SaveDefualtConfigFile() => SaveConfigFile(PathMgr.GetAppConfigFile());

        public void SaveConfigFile(string file)
        {
            string contents = JsonConvert.SerializeObject(AppConfig, Formatting.Indented);
            File.WriteAllText(file, contents);
        }

        public void InitUserConfig()
        {
            if (UserConfigExist(Session.DefaultConfig))
                return;
            File.Copy(PathMgr.GetAppConfigFile(), PathMgr.DefaultUserConfigFile);
        }

        public void SaveUserConfig(string configName)
        {
            SaveConfigFile(Path.Combine(PathMgr.UserConfigDir, configName));
        }

        public string GetUserConfigPath(string configName)
        {
            return Path.Combine(PathMgr.UserConfigDir, configName);
        }

        private AppConfigMgr() => AppConfig = new AppConfig();

        private void LoadConfig() => LoadConfig(PathMgr.GetAppConfigFile());

        public void LoadDefaultConfig() => LoadConfig();

        public void LoadUserConfig(string configName)
        {
            LoadConfig(Path.Combine(PathMgr.UserConfigDir, configName));
        }

        public void LoadConfig(string file)
        {
            if (!File.Exists(file))
                return;
            AppConfig = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(file), (JsonConverter)new TargetObjectJsonConverter());
        }

        public List<string> GetAllUserConfig()
        {
            List<string> allUserConfig = new List<string>();
            foreach (string file in Directory.GetFiles(PathMgr.UserConfigDir))
                allUserConfig.Add(Path.GetFileName(file));
            return allUserConfig;
        }

        public bool UserConfigExist(string configName)
        {
            return File.Exists(Path.Combine(PathMgr.UserConfigDir, configName));
        }

        public void CreateNewUserConfigFrom(string configName, string from)
        {
            if (File.Exists(Path.Combine(PathMgr.UserConfigDir, configName)))
                return;
            File.Copy(from, Path.Combine(PathMgr.UserConfigDir, configName));
        }

        public void CreateUserConfig(string name)
        {
            CreateNewUserConfigFrom(name, PathMgr.GetAppConfigFile());
        }
    }
}
