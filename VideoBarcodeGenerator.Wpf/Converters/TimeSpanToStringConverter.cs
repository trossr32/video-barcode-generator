using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoBarcodeGenerator.Wpf.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeSpan?)value)?.ToString(@"hh\:mm\:ss") ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}