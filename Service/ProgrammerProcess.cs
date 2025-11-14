using System.Runtime.InteropServices;
using static Service.ProgrammerProcess;

namespace Service
{

	public static class ProgrammerProcess
	{
		[DllImport("Flash.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int ExecuteProgrammerForCSharp(
			string configFile,
			ProgrammerHandlerCallback handlers,
			string cklink);

		public static int ExecuteProgrammer(
			string configFile,
			ProgrammerHandlerCallback handlers,
			string cklink = "default")
		{
			return ExecuteProgrammerForCSharp(configFile, handlers, cklink);
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate bool IsCanceled();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnUpdateInfo(string info);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnError(string msg);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnWarning(string msg);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void OnWork(int worked);

		public readonly struct ProgrammerHandlerCallback
		{
			private readonly IsCanceled isCanceled;
			private readonly OnUpdateInfo onUpdateInfo;
			private readonly OnError onError;
			private readonly OnWarning onWarning;
			private readonly OnWork onWork;

			public ProgrammerHandlerCallback(
				IsCanceled Canceled,
				OnUpdateInfo UpdateInfo,
				OnError Error,
				OnWarning Warning,
				OnWork Work)
			{

				isCanceled = Canceled;
				onUpdateInfo = UpdateInfo;
				onError = Error;
				onWarning = Warning;
				onWork = Work;
			}
		}
	}
}
