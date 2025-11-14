using CskyFlashProgramer.UI;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CskyFlashProgramer
{
    public partial class MainConfigControl : UserControl, IComponentConnector
    {
        public MainConfigControl()
        {
            InitializeComponent();
            m_startButton.Command = new ProgrammerWriteCmd();
        }
    }
}
