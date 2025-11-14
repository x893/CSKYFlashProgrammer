using MahApps.Metro.Controls.Dialogs;
using System;

namespace Service
{
    public class FlashProgrammerHandler
    {
        private static ProgressDialogController DialogController;
        private static int alreadywork;

        public static ProgrammerProcess.ProgrammerHandlerCallback Create(
            ProgressDialogController controller)
        {
            alreadywork = 0;
            DialogController = controller;
            return new ProgrammerProcess.ProgrammerHandlerCallback(
                new ProgrammerProcess.IsCanceled(IsCanceled),
                new ProgrammerProcess.OnUpdateInfo(OnUpdateInfo),
                new ProgrammerProcess.OnError(OnError),
                new ProgrammerProcess.OnWarning(OnWarning),
                new ProgrammerProcess.OnWork(OnWork)
                );
        }

        private static bool IsCanceled() => DialogController.IsCanceled;

        private static void OnUpdateInfo(string info)
        {
            DialogController.SetMessage(info);
        }

        private static void OnError(string msg)
        {
            if (!IsCanceled())
                throw new Exception(msg);
        }

        private static void OnWarning(string msg)
        {
            if (!IsCanceled())
                throw new WarningException(msg);
        }

        private static void OnWork(int worked)
        {
            alreadywork += worked;
            double maximum = DialogController.Maximum;
            if (alreadywork >= maximum - 10.0)
                return;
            DialogController.SetProgress(alreadywork);
        }
    }
}
