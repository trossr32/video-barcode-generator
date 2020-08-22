using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoBarcodeGenerator.Wpf.Converters
{
    public class ShallowHeightIfTrueButtonIconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool?)value ?? false ? 19 : 32;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}