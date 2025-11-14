using System;

namespace FlashProgramerConsole
{
    public static class ConsoleProgrammerHandlers
    {
        public static bool Cancel;

        public static bool IsCanceled() => Cancel;

        public static void OnUpdateInfo(string info) => Console.WriteLine(info);

        public static void OnError(string msg)
        {
            if (!IsCanceled())
                throw new Exception(msg);
        }

        public static void OnWarning(string msg)
        {
            if (IsCanceled())
                return;
            Console.WriteLine(msg);
        }

        public static void OnWork(int worked)
        {
        }
    }
}
