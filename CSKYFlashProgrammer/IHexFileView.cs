using CskyFlashProgramer.UI;
using Service;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CskyFlashProgramer
{

    public sealed class IHexFileView : Elf_iHexFilePicker, IPageUI
    {
        private readonly HexObject HexObj;

        public IHexFileView(HexObject obj)
        {
            HexObj = obj;
            m_filePath.SetCRCTextProperty(m_crc16, m_crc32);
            m_filePath.Filter = FileFilterUtils.HexFiles + FileFilterUtils.Seperator + FileFilterUtils.AllFiles;
            groupBox.Header = "Hex";
            SetBindingUtils.SetGeneralBinding(HexObj, "Program", m_program, ToggleButton.IsCheckedProperty);
            SetBindingUtils.SetGeneralBinding(HexObj, "Verify", m_verify, ToggleButton.IsCheckedProperty);
            m_filePath.SetBinding(new Binding()
            {
                Source = HexObj,
                Path = new PropertyPath("FilePath", new object[0])
            });
        }

        public TargetObject GetObj() => HexObj;
    }
}