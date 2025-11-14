using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{
    [ValueConversion(typeof(string), typeof(bool))]
    public sealed class EnableConverter : IValueConverter
    {
        public static readonly EnableConverter Default = new EnableConverter();

        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value.Equals(EraseTypeUtil.EraseRangStr);
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