using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{

    [ValueConversion(typeof(DumpFileType), typeof(string))]
    public sealed class DumpFileTypeConverter : IValueConverter
    {
        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value.ToString();
        }

        object IValueConverter.ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return DumpFileTypeUtil.StrToEnum(value as string);
        }
    }
}