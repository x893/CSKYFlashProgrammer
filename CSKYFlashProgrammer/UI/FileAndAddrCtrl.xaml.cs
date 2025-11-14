using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{
    public abstract partial class FileAndAddrCtrl : UserControl, IComponentConnector
    {
        public FileAndAddrCtrl()
        {
            InitializeComponent();
            InitDumpPathCtrl();
            m_readButton.Command = new ProgrammerReadCmd();
        }

        protected abstract void InitDumpPathCtrl();

        protected void DoInitDumpPathCtrl(DumpFileInfo source)
        {
            m_savePath.SetBinding(new Binding()
            {
                Source = source,
                Path = new PropertyPath("FilePath", new object[0])
            });
            SetBindingUtils.SetHexNumTextboxBinding(source, "Length", m_readLength);
            SetBindingUtils.SetHexNumTextboxBinding(source, "StartAddress", m_readStart);
        }
    }
}