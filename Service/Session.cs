namespace Service
{
    public class Session
    {
        public string Theme = "Blue";
        public static readonly string DefaultConfig = "default";
        private string _UserSelectedConfig = DefaultConfig;

        public string UserSelectedConfig
        {
            get
            {
                return !AppConfigMgr.Instance.UserConfigExist(_UserSelectedConfig) ? DefaultConfig : _UserSelectedConfig;
            }
            set => _UserSelectedConfig = value;
        }
    }
}
