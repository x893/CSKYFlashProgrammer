using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace CskyFlashProgramer
{
    public partial class Elf_iHexFilePicker : UserControl, IComponentConnector
    {
        public Elf_iHexFilePicker() => this.InitializeComponent();

        private void OnProgramClicked(object sender, RoutedEventArgs e) => this.UpdateView();

        private void OnVerifyClicked(object sender, RoutedEventArgs e) => this.UpdateView();

        private void UpdateView()
        {
            if (!this.m_program.IsChecked.Value && !this.m_verify.IsChecked.Value)
            {
                this.m_filePath.IsEnabled = false;
            }
            else
            {
                if (!this.m_program.IsChecked.Value && !this.m_verify.IsChecked.Value)
                    return;
                this.m_filePath.IsEnabled = true;
            }
        }
    }
}