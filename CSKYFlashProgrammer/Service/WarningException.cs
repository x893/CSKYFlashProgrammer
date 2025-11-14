using System;

namespace Service
{
	public class WarningException : Exception
	{
		public WarningException(string msg) : base(msg)
		{ }
	}
}
