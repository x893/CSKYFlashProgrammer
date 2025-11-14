using System;

namespace Service
{
	public class TargetObject : ICloneable
	{
		public ProgramFileType Type;
		public bool Program { get; set; }
		public TargetObject() => Program = true;
		public object Clone() => MemberwiseClone();
	}
}
