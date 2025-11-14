using System;
using System.Collections.Generic;

namespace Service
{

    public class ConnectTypeUtil
    {
        public static ConnectType StringToEnum(string str)
        {
            return (ConnectType)Enum.Parse(typeof(ConnectType), str);
        }

        public static List<string> GetList()
        {
            return new List<string>()
            {
                ConnectType.Normal.ToString(),
                ConnectType.HardReset.ToString()
            };
        }
    }
}
