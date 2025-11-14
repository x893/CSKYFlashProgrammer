using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;

namespace CskyFlashProgramer.UI
{
	public partial class TargetFileReadView : UserControl, IComponentConnector
	{
		public TargetFile TargetFile => AppConfigMgr.Instance.AppConfig.FlashProgramSession.TargetFile;

		public TargetFileReadView()
		{
			InitializeComponent();
			InitDumpFileCombobox();
		}

		private void InitDumpFileCombobox()
		{
			m_readFileType.Items.Clear();
			m_readFileType.ItemsSource = DumpFileTypeUtil.GetList();
			m_readFileType.SetBinding(Selector.SelectedValueProperty, new Binding()
			{
				Source = TargetFile,
				Path = new PropertyPath("DumpFileType", new object[0]),
				Converter = new DumpFileTypeConverter()
			});
		}
	}
}