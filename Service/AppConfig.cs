namespace Service
{
    public class AppConfig
    {
        public DebugServerSetting DebugServerSetting { get; set; }

        public FlashProgramSession FlashProgramSession { get; set; }

        public AppConfig()
        {
            DebugServerSetting = new DebugServerSetting();
            FlashProgramSession = new FlashProgramSession();
        }
    }
}