using Service;

namespace CskyFlashProgramer.UI
{

    public class ProgrammerWriteCmd : ProgrammerCmdBase
    {
        protected override void DoStartProgram(object parameter)
        {
            FlashProgramSession.CommandType = CommandType.Write;
            base.DoStartProgram(parameter);
        }
    }
}