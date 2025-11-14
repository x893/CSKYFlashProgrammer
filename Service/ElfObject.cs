using System;
using System.IO;

namespace Service
{
    public class ElfObject : TargetObject
    {
        public bool Verify { get; set; }

        public string FilePath { get; set; }

        public ElfObject()
        {
            Type = ProgramFileType.Elf;
            Verify = false;
            FilePath = "";
        }

        public uint GetFileSize() => Convert.ToUInt32(new FileInfo(FilePath).Length);
    }
}
