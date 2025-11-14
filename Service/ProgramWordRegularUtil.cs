using System;
using System.Collections.Generic;

namespace Service
{
    public class ProgramWordRegularUtil
    {
        public static List<string> GetList()
        {
            return new List<string>()
            {
                ProgramWordRegular.None.ToString(),
                ProgramWordRegular.Up.ToString(),
                ProgramWordRegular.Down.ToString()
            };
        }

        public static ProgramWordRegular StrToEnum(string str)
        {
            return (ProgramWordRegular)Enum.Parse(typeof(ProgramWordRegular), str);
        }
    }
}
