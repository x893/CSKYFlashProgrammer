using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Service
{

	public class RegisterConverter
	{
		private static RegisterConverter _registerConverter;
		private readonly string ConfigFilePathV1 = "config/register_conversion_v1.json";
		private readonly string ConfigFilePathV2 = "config/register_conversion_v2.json";

		public Dictionary<string, uint> RegDictionaryV1 { get; protected set; }

		public Dictionary<string, uint> RegDictionaryV2 { get; protected set; }

		public static RegisterConverter Instance()
		{
			if (_registerConverter == null)
				_registerConverter = new RegisterConverter();
			return _registerConverter;
		}

		public uint GetRegNo(string regName, ABI abi)
		{
			if (abi == ABI.ABIV1)
				return RegDictionaryV1.ContainsKey(regName) ? RegDictionaryV1[regName] : throw new RegisterConvertException("Not have register " + regName);
			return RegDictionaryV2.ContainsKey(regName) ? RegDictionaryV2[regName] : throw new RegisterConvertException("Not have register " + regName);
		}

		public void Save()
		{
			File.WriteAllText(ConfigFilePathV1, JsonConvert.SerializeObject((object)RegDictionaryV1, Formatting.Indented));
			File.WriteAllText(ConfigFilePathV2, JsonConvert.SerializeObject((object)RegDictionaryV2, Formatting.Indented));
		}

		private void Create() => LoadFromJson();

		private void LoadFromJson()
		{
			if (File.Exists(ConfigFilePathV1))
				RegDictionaryV1 = JsonConvert.DeserializeObject<Dictionary<string, uint>>(File.ReadAllText(ConfigFilePathV1));
			else
				MakeDefaultV1();
			if (File.Exists(ConfigFilePathV2))
				RegDictionaryV2 = JsonConvert.DeserializeObject<Dictionary<string, uint>>(File.ReadAllText(ConfigFilePathV2));
			else
				MakeDefaultV2();
		}

		private void MakeDefaultV1()
		{
			RegDictionaryV1 = new Dictionary<string, uint>()
			{
				{ "r0", 0U },
				{ "r1", 1U }
			};
		}

		private void MakeDefaultV2()
		{
			RegDictionaryV2 = new Dictionary<string, uint>()
			{
				{ "r0", 0U },
				{ "r1", 1U }
			};
		}

		private RegisterConverter() => Create();
	}
}