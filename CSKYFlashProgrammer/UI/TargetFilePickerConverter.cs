using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{

	[ValueConversion(typeof(string), typeof(object))]
	internal class TargetFilePickerConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.Equals(string.Empty))
				return new ElfFileView(new ElfObject());
			switch ((ProgramFileType)Enum.Parse(typeof(ProgramFileType), (string)value))
			{
				case ProgramFileType.Elf:
					return new ElfFileView(new ElfObject());
				case ProgramFileType.Hex:
					return new IHexFileView(new HexObject());
				case ProgramFileType.Bin:
				case ProgramFileType.Word:
					return new WordValuePage(new WordObject());
				default:
					throw new NotImplementedException();
			}
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
