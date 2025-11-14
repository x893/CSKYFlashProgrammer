using Service;

namespace CskyFlashProgramer.UI
{
    public class DumpBinFileCtrl : FileAndAddrCtrl
    {
        protected override void InitDumpPathCtrl()
        {
            DoInitDumpPathCtrl(AppConfigMgr.Instance.AppConfig.FlashProgramSession.TargetFile.DumpBinFileInfo);
            m_savePath.Filter = FileFilterUtils.BinFiles;
        }
    }
}