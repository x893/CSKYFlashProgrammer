using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{

	[ValueConversion(typeof(uint), typeof(string))]
	internal class HexToStringConverter : IValueConverter
	{
		public HexToStringConverter()
		{
			AddPrefix = true;
			Prefix = "0x";
		}

		public HexToStringConverter SetIfAddPrefix(bool b)
		{
			AddPrefix = b;
			return this;
		}

		public bool AddPrefix { get; set; }

		public string Prefix { get; set; }

		object IValueConverter.Convert(
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			string empty = string.Empty;
			if (AddPrefix)
				empty += Prefix;
			return (empty + Convert.ToString((uint)value, 0x10));
		}

		object IValueConverter.ConvertBack(
			object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			try
			{
				return Convert.ToUInt32((string)value, 0x10);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}