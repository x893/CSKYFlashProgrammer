using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{

    [ValueConversion(typeof(ProgramWordRegular), typeof(string))]
    public sealed class ProgramWordTypeConverter : IValueConverter
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
            return ProgramWordRegularUtil.StrToEnum(value as string);
        }
    }
}