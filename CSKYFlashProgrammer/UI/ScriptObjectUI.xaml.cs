using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{

    public partial class ScriptObjectUI : UserControl, IPageUI, IComponentConnector
    {
        private readonly ScriptObject ScriptObj;

        public TargetObject GetObj() => ScriptObj;

        public ScriptObjectUI(ScriptObject obj)
        {
            ScriptObj = obj;
            InitializeComponent();
            m_filePicker.Filter = FileFilterUtils.ScriptFiles + FileFilterUtils.Seperator + FileFilterUtils.AllFiles;
            SetBindingUtils.SetGeneralBinding(ScriptObj, "Program", m_program, ToggleButton.IsCheckedProperty);
            m_filePicker.SetBinding(new Binding()
            {
                Source = ScriptObj,
                Path = new PropertyPath("FilePath", new object[0])
            });
        }

        private void OnProgramClicked(object sender, RoutedEventArgs e)
        {
            if (!m_program.IsChecked.Value)
                m_filePicker.IsEnabled = false;
            else
                m_filePicker.IsEnabled = true;
        }
    }
}