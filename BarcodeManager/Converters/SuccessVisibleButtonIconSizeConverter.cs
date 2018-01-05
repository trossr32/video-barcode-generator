using System;
using System.Globalization;
using System.Windows.Data;

namespace BarcodeManager.Converters
{
    public class SuccessVisibleButtonIconSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && (bool) value ? 22 : 32;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}