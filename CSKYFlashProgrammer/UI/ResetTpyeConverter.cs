using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{
    [ValueConversion(typeof(ResetType), typeof(string))]
    public sealed class ResetTpyeConverter : IValueConverter
    {
        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return ResetTypeUtil.ResetTypeToString((ResetType)value);
        }

        object IValueConverter.ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return ResetTypeUtil.StringToEnum(value as string);
        }
    }
}