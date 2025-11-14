using MahApps.Metro.Controls.Dialogs;
using Service;
using System.IO;
using System.Windows;

namespace CskyFlashProgramer.UI
{

    public class ProgrammerReadCmd : ProgrammerCmdBase
    {
        protected override void DoStartProgram(object parameter)
        {
            FlashProgramSession.CommandType = CommandType.Read;
            base.DoStartProgram(parameter);

        }

        protected override bool DoCheck() => CheckDumpFileExsits();

        private bool CheckDumpFileExsits()
        {
            if (File.Exists(FlashProgramSession.TargetFile.GetDumpFileInfo().FilePath))
            {
                switch ((Application.Current.MainWindow as MainWindow).ShowModalMessageExternal(
                    "Warning",
                    $"{FlashProgramSession.TargetFile.GetDumpFileInfo().FilePath} file already exists, do you want to replace it?",
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No"
                    }))
                {
                    case MessageDialogResult.Negative:
                        return false;
                    case MessageDialogResult.Affirmative:
                        return true;
                }
            }
            return true;
        }
    }
}