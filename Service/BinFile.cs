using System;
using System.IO;

namespace Service
{

    public class BinFile : ICloneable
    {
        public uint Offset { get; set; }

        public string FilePath { get; set; }

        public BinFile()
        {
            Offset = 0U;
            FilePath = "";
        }

        public uint GetFileSize() => Convert.ToUInt32(new FileInfo(FilePath).Length);

        public object Clone() => MemberwiseClone();
    }
}
