using Plossum.CommandLine;
using Service;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FlashProgramerConsole
{

    internal class Program
    {
        private const int FormatWidth = 78;

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if (ctrlType == CtrlTypes.CTRL_C_EVENT)
                ConsoleProgrammerHandlers.Cancel = true;
            return true;
        }

        private static int Main(string[] args)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            CommandLineOptions commandLineOptions = new CommandLineOptions();
            CommandLineParser commandLineParser = new CommandLineParser(commandLineOptions);
            commandLineParser.Parse();
            Console.WriteLine(commandLineParser.UsageInfo.GetHeaderAsString(FormatWidth));
            if (commandLineOptions.Help)
            {
                Console.WriteLine(commandLineParser.UsageInfo.GetOptionsAsString(FormatWidth));
                Console.WriteLine(GetConfigFileFormatHelp());
                return 0;
            }
            if (commandLineParser.HasErrors)
            {
                Console.WriteLine(commandLineParser.UsageInfo.GetErrorsAsString(FormatWidth));
                return -1;
            }
            try
            {
                StartProgram(commandLineOptions);
                string str = "Flash program success.";
                if (ConsoleProgrammerHandlers.Cancel)
                    str = "User canceled operation.";
                Console.WriteLine(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return 0;
        }

        private static ProgrammerProcess.ProgrammerHandlerCallback HandelerCall { get; set; }

        private static void StartProgram(CommandLineOptions options)
        {
            HandelerCall = new ProgrammerProcess.ProgrammerHandlerCallback(
                new ProgrammerProcess.IsCanceled(ConsoleProgrammerHandlers.IsCanceled),
                new ProgrammerProcess.OnUpdateInfo(ConsoleProgrammerHandlers.OnUpdateInfo),
                new ProgrammerProcess.OnError(ConsoleProgrammerHandlers.OnError),
                new ProgrammerProcess.OnWarning(ConsoleProgrammerHandlers.OnWarning),
                new ProgrammerProcess.OnWork(ConsoleProgrammerHandlers.OnWork)
                );
            ProgrammerProcess.ExecuteProgrammer(options.ConfigFile, HandelerCall, options.CKLink);
        }

        private static string GetConfigFileFormatHelp()
        {
            return File.Exists(PathMgr.GetConsoleHelpFile()) ? File.ReadAllText(PathMgr.GetConsoleHelpFile()) : "Not have help file.";
        }

        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6,
        }
    }
}
