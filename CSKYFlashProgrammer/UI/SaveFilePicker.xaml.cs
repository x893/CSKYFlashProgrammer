using Microsoft.Win32;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{
    public partial class SaveFilePicker : UserControl, IPickerDlg, IComponentConnector
    {
        public string Path
        {
            get => m_textBox.Text;
            set => m_textBox.Text = value;
        }

        public string Filter { set; get; }

        public string InitialDirectory { get; set; }

        bool IPickerDlg.Multiselect { get; set; }

        public SaveFilePicker()
        {
            Filter = "All files (*.*)|*.*";
            InitializeComponent();
        }

        public void SetBinding(Binding binding)
        {
            m_textBox.SetBinding(TextBox.TextProperty, binding);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			SaveFileDialog dlg = new SaveFileDialog
			{
				AddExtension = true,
				Filter = Filter
			};
			FilePicker.SetInitDir(dlg, this);
            bool? nullable = dlg.ShowDialog();
            bool flag = true;
            if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                return;
            m_textBox.Text = PathMgr.MakeRelativeToRuntimeDir(dlg.FileName);
            m_textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}