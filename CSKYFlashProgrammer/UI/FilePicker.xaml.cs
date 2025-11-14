using Microsoft.Win32;
using Service;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{
    public partial class FilePicker : UserControl, IPickerDlg, IComponentConnector
    {
        private TextBox m_crc16;
        private TextBox m_crc32;

        public string Path
        {
            get => m_textBox.Text;
            set => m_textBox.Text = value;
        }

        public string Filter { set; get; }

        public string InitialDirectory { get; set; }

        public bool Multiselect { get; set; }

        public FilePicker()
        {
            Filter = "All files (*.*)|*.*";
            Multiselect = false;
            InitializeComponent();
        }

        public void SetBinding(Binding binding)
        {
            m_textBox.SetBinding(TextBox.TextProperty, binding);
        }

        public void SetCRCTextProperty(TextBox crc16, TextBox crc32)
        {
            m_crc16 = crc16;
            m_crc32 = crc32;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Filter;
            SetInitDir(dlg, this);
            dlg.Multiselect = Multiselect;
            dlg.CheckFileExists = true;
            dlg.CheckPathExists = true;
            try
            {
                bool? nullable = dlg.ShowDialog();
                bool flag = true;
                if ((nullable.GetValueOrDefault() == flag ? (nullable.HasValue ? 1 : 0) : 0) == 0)
                    return;
                m_textBox.Text = PathMgr.MakeRelativeToRuntimeDir(dlg.FileName);
                m_textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void Text_Changed(object sender, RoutedEventArgs e)
        {
            if (m_crc16 == null || m_crc32 == null)
                return;
            m_crc16.Text = CalcCRC.GetCRCValue(m_textBox.Text, CRCType.CRC16);
            m_crc32.Text = CalcCRC.GetCRCValue(m_textBox.Text, CRCType.CRC32);
        }

        public static void SetInitDir(FileDialog dlg, IPickerDlg pickerDlg)
        {
            if (pickerDlg.InitialDirectory != null)
                dlg.InitialDirectory = pickerDlg.InitialDirectory;
            else if (!pickerDlg.Path.Equals(string.Empty))
            {
                string directoryName = System.IO.Path.GetDirectoryName(pickerDlg.Path);
                if (!Directory.Exists(directoryName))
                    return;
                dlg.InitialDirectory = System.IO.Path.GetFullPath(directoryName);
            }
            else
                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }
    }
}