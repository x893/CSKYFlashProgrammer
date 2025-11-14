using System;
using System.Collections.Generic;

namespace Service
{
    public class DumpFileTypeUtil
    {
        public static List<string> GetList()
        {
            return new List<string>()
    {
      DumpFileType.Hex.ToString(),
      DumpFileType.Bin.ToString()
    };
        }

        public static DumpFileType StrToEnum(string str)
        {
            return (DumpFileType)Enum.Parse(typeof(DumpFileType), str);
        }
    }
}
