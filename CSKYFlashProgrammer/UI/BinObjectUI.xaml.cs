using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{

    public partial class BinObjectUI : UserControl, IPageUI, IComponentConnector
    {
        private readonly BinObject BinObj;
        public BinObjectUI(BinObject Obj)
        {
            BinObj = Obj;
            InitializeComponent();
            SetBindingUtils.SetHexNumTextboxBinding(BinObj, "TargetAddr", m_offset);
            SetBindingUtils.SetGeneralBinding(BinObj, "Program", m_program, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(BinObj, "Verify", m_verify, ToggleButton.IsCheckedProperty);
            m_filePicker.SetCRCTextProperty(m_crc16, m_crc32);
            m_filePicker.Filter = FileFilterUtils.BinFiles + FileFilterUtils.Seperator + FileFilterUtils.AllFiles;
            m_filePicker.SetBinding(new Binding()
            {
                Source = BinObj,
                Path = new PropertyPath("FilePath", new object[0])
            });
        }

        public TargetObject GetObj() => BinObj;

        private void OnProgramClicked(object sender, RoutedEventArgs e) => UpdateView();

        private void UpdateView()
        {
            if (!m_program.IsChecked.Value && !m_verify.IsChecked.Value)
            {
                m_offset.IsEnabled = false;
                m_filePicker.IsEnabled = false;
            }
            else
            {
                if (!m_program.IsChecked.Value && !m_verify.IsChecked.Value)
                    return;
                m_offset.IsEnabled = true;
                m_filePicker.IsEnabled = true;
            }
        }

        private void OnVerifyClicked(object sender, RoutedEventArgs e) => UpdateView();

    }
}