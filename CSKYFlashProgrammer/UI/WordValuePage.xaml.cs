using Service;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{

    public partial class WordValuePage : UserControl, IPageUI, IComponentConnector
    {
        public WordObject WordObj { get; set; }

        public TargetObject GetObj() => WordObj;

        public WordValuePage(WordObject obj)
        {
            WordObj = obj;
            InitializeComponent();
            SetBindingUtils.SetGeneralBinding(WordObj, "Program", m_program, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(WordObj, "Verify", m_verify, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetHexNumTextboxBinding(WordObj, "Start", m_wordStart);
            SetBindingUtils.SetHexNumTextboxBinding(WordObj, "Length", m_wordLength);
            SetBindingUtils.SetHexNumTextboxBinding(WordObj, "Value", m_wordValue);
            m_wordType.Items.Clear();
            m_wordType.ItemsSource = ProgramWordRegularUtil.GetList();
            m_wordType.SetBinding(Selector.SelectedValueProperty, new Binding()
            {
                Source = WordObj,
                Converter = new ProgramWordTypeConverter(),
                Path = new PropertyPath("Regular", new object[0])
            });
        }

        private void OnProgramClicked(object sender, RoutedEventArgs e) => UpdateView();

        private void OnVerifyClicked(object sender, RoutedEventArgs e) => UpdateView();

        private void UpdateView()
        {
            if (!m_program.IsChecked.Value && !m_verify.IsChecked.Value)
            {
                m_wordStart.IsEnabled = false;
                m_wordValue.IsEnabled = false;
                m_wordLength.IsEnabled = false;
                m_wordType.IsEnabled = false;
            }
            else
            {
                if (!m_program.IsChecked.Value && !m_verify.IsChecked.Value)
                    return;
                m_wordStart.IsEnabled = true;
                m_wordValue.IsEnabled = true;
                m_wordLength.IsEnabled = true;
                m_wordType.IsEnabled = true;
            }
        }
    }
}