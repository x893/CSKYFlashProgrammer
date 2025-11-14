using System;
using System.IO;

namespace Service
{
    public class ScriptObject : TargetObject
    {
        public string FilePath { get; set; }

        public ScriptObject()
        {
            Type = ProgramFileType.Script;
            FilePath = "";
        }

        public uint GetFileSize() => Convert.ToUInt32(new FileInfo(FilePath).Length);
    }
}
