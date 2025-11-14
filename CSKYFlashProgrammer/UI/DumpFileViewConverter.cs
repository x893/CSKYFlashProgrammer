using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{

	[ValueConversion(typeof(string), typeof(object))]
	internal class DumpFileViewConverter : IValueConverter
	{
		object IValueConverter.Convert(
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value as string))
				return new DumpBinFileCtrl();
			object obj;
			switch (DumpFileTypeUtil.StrToEnum(value as string))
			{
				case DumpFileType.Hex:
					obj = new DumpHexFileCtrl();
					break;
				case DumpFileType.Bin:
					obj = new DumpBinFileCtrl();
					break;
				default:
					obj = new DumpBinFileCtrl();
					break;
			}
			return obj;
		}

		object IValueConverter.ConvertBack(
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
