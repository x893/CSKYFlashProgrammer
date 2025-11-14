using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{

    [ValueConversion(typeof(EraseType), typeof(string))]
    public sealed class EraseTpyeConverter : IValueConverter
    {
        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return EraseTypeUtil.EraseTypeToString((EraseType)value);
        }

        object IValueConverter.ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return EraseTypeUtil.StringToEnum(value as string);
        }
    }
}
