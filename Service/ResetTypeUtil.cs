using System;
using System.Collections.Generic;

namespace Service
{
    public class ResetTypeUtil
    {
        public static readonly string HardStr = "Hard";
        public static readonly string SoftStr = "Soft";
        private static readonly Dictionary<ResetType, string> m_map = new Dictionary<ResetType, string>()
        {
            {
                ResetType.Soft,
                SoftStr
            },
            {
                ResetType.Hard,
                HardStr
            }
        };

        public static List<string> GetAllTypeAsStrings()
        {
            List<string> allTypeAsStrings = new List<string>();
            foreach (KeyValuePair<ResetType, string> keyValuePair in m_map)
                allTypeAsStrings.Add(keyValuePair.Value);
            return allTypeAsStrings;
        }

        public static string ResetTypeToString(ResetType resetType) => m_map[resetType];

        public static ResetType StringToEnum(string str)
        {
            foreach (KeyValuePair<ResetType, string> keyValuePair in m_map)
            {
                if (str.Equals(keyValuePair.Value))
                    return keyValuePair.Key;
            }
            throw new Exception("ResetTypeUtil: not have this reset type!");
        }
    }
}
