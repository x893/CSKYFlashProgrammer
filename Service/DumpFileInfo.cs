using System;

namespace Service
{
	public class DumpFileInfo : ICloneable
	{
		public string FilePath { get; set; } = "";

		public uint StartAddress { get; set; }

		public uint Length { get; set; }

		public object Clone() => MemberwiseClone();
	}
}
