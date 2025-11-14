using System;
using System.IO;

namespace Service
{
    public class BinObject : TargetObject
    {
        public bool Verify { get; set; }

        public uint TargetAddr { get; set; }

        public string FilePath { get; set; }

        public BinObject()
        {
            Type = ProgramFileType.Bin;
            TargetAddr = 0U;
            FilePath = "";
            Verify = false;
        }

        public uint GetFileSize() => Convert.ToUInt32(new FileInfo(FilePath).Length);
    }
}
