using System;

namespace Service
{
    public class TargetFile : ICloneable
    {
        public TargetFile()
        {
            DumpFileType = DumpFileType.Hex;
            DumpBinFileInfo = new DumpFileInfo();
            DumpHexFileInfo = new DumpFileInfo();
        }

        public DumpFileType DumpFileType { get; set; }

        public DumpFileInfo DumpBinFileInfo { get; set; }

        public DumpFileInfo DumpHexFileInfo { get; set; }

        public DumpFileInfo GetDumpFileInfo()
        {
            switch (DumpFileType)
            {
                case DumpFileType.Hex:
                    return DumpHexFileInfo;
                case DumpFileType.Bin:
                    return DumpBinFileInfo;
                default:
                    throw new NotImplementedException();
            }
        }

        public object Clone()
        {
            TargetFile targetFile = MemberwiseClone() as TargetFile;
            targetFile.DumpHexFileInfo = DumpHexFileInfo.Clone() as DumpFileInfo;
            targetFile.DumpBinFileInfo = DumpBinFileInfo.Clone() as DumpFileInfo;
            return targetFile;
        }
    }
}
