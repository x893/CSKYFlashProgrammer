namespace Service
{
	public class DebugServerSetting
	{
		public bool UseDDC { get; set; }

		public bool EnableTRST { get; set; }

		public bool EnableDbgSvrLog { get; set; }

		public uint ICECLk { get; set; }

		public uint DelayMTCR { get; set; }
	}
}