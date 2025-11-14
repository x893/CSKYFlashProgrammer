namespace CskyFlashProgramer.UI
{
	public interface IPickerDlg
	{
		string Path { set; get; }

		string Filter { set; get; }

		string InitialDirectory { get; set; }

		bool Multiselect { get; set; }
	}
}
