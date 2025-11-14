using Service;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CskyFlashProgramer.UI
{
    [ValueConversion(typeof(string), typeof(bool))]
    public sealed class ResetEnableConverter : IValueConverter
    {
        private readonly TargetConfig m_config;

        public ResetEnableConverter(TargetConfig config) => m_config = config;

        object IValueConverter.Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value.Equals(ResetType.Soft) && m_config.ResetAndRun;
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
