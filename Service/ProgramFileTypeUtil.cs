using System;
using System.Collections.Generic;

namespace Service
{
    public class ProgramFileTypeUtil
    {
        public static List<string> GetList()
        {
            return new List<string>()
            {
                ProgramFileType.Bin.ToString(),
                ProgramFileType.Hex.ToString(),
                ProgramFileType.Elf.ToString(),
                ProgramFileType.Word.ToString()
            };
        }

        public static ProgramFileType StrToEnum(string str)
        {
            return (ProgramFileType)Enum.Parse(typeof(ProgramFileType), str);
        }
    }
}
