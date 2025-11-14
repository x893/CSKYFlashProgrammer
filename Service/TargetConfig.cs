using System;

namespace Service
{
    public class TargetConfig : ICloneable
    {
        public string AlgorithmFile { set; get; }
        public string PreScriptFile { set; get; }
        public EraseType EraseType { set; get; }
        public uint EraseRangStart { set; get; }
        public uint EraseRangLength { set; get; }
        public ConnectType ConnectType { set; get; }
        public uint Timeout { set; get; }
        public bool LogEnable { set; get; }
        public bool ResetAndRun { set; get; }
        public ResetType ResetType { set; get; }
        public uint ResetValue { set; get; }
        public bool RunMode { set; get; }
        public uint CPUSelect { set; get; }

        public TargetConfig()
        {
            AlgorithmFile = "";
            EraseType = EraseType.EraseSectors;
            EraseRangStart = 0U;
            EraseRangLength = 0U;
            ConnectType = ConnectType.Normal;
            Timeout = 60U;
            LogEnable = false;
            ResetAndRun = true;
            RunMode = false;
            ResetType = ResetType.Hard;
            ResetValue = 2882343476U;
            CPUSelect = 0U;
        }

        public object Clone() => MemberwiseClone();
    }
}
