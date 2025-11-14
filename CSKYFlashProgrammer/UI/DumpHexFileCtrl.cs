using Service;

namespace CskyFlashProgramer.UI
{
    public class DumpHexFileCtrl : FileAndAddrCtrl
    {
        protected override void InitDumpPathCtrl()
        {
            DoInitDumpPathCtrl(AppConfigMgr.Instance.AppConfig.FlashProgramSession.TargetFile.DumpHexFileInfo);
            m_savePath.Filter = FileFilterUtils.HexFiles;
        }
    }
}
