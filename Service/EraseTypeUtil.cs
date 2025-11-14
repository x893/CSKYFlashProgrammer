using System;
using System.Collections.Generic;

namespace Service
{
    public class EraseTypeUtil
    {
        public static readonly string EraseRangStr = "Erase Range";
        public static readonly string ChipEraseStr = "Chip Erase";
        public static readonly string EraseSectorsStr = "Erase Sectors";
        public static readonly string NotEraseStr = "Not Erase";
        private static Dictionary<EraseType, string> m_map = new Dictionary<EraseType, string>()
        {
            {
                EraseType.ChipErase,
                ChipEraseStr
            },
            {
                EraseType.EraseSectors,
                EraseSectorsStr
            },
            {
                EraseType.EraseRange,
                EraseRangStr
            },
            {
                EraseType.NotErase,
                NotEraseStr
            }
        };

        public static List<string> GetAllTypeAsStrings()
        {
            List<string> allTypeAsStrings = new List<string>();
            foreach (KeyValuePair<EraseType, string> keyValuePair in m_map)
                allTypeAsStrings.Add(keyValuePair.Value);
            return allTypeAsStrings;
        }

        public static string EraseTypeToString(EraseType eraseType) => m_map[eraseType];

        public static EraseType StringToEnum(string str)
        {
            foreach (KeyValuePair<EraseType, string> keyValuePair in m_map)
            {
                if (str.Equals(keyValuePair.Value))
                    return keyValuePair.Key;
            }
            throw new Exception("EraseTypeUtil: not have this erase type!");
        }
    }
}
