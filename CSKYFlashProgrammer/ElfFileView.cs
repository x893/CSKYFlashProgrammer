using CskyFlashProgramer.UI;
using Service;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CskyFlashProgramer
{
    public sealed class ElfFileView : Elf_iHexFilePicker, IPageUI
    {
        private readonly ElfObject ElfObj;

        public ElfFileView(ElfObject obj)
        {
            ElfObj = obj;
            m_filePath.SetCRCTextProperty(m_crc16, m_crc32);
            m_filePath.Filter = FileFilterUtils.ElfFiles + FileFilterUtils.Seperator + FileFilterUtils.AllFiles;
            groupBox.Header = "Elf";
            SetBindingUtils.SetGeneralBinding(ElfObj, "Program", m_program, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(ElfObj, "Verify", m_verify, ToggleButton.IsCheckedProperty);
            m_filePath.SetBinding(new Binding()
            {
                Source = ElfObj,
                Path = new PropertyPath("FilePath", new object[0])
            });
        }

        public TargetObject GetObj() => ElfObj;
    }
}
