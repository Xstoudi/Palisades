using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Palisades.Converters
{
    internal class SolidBrushToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush colorBrush = (SolidColorBrush)value;
            return colorBrush.Color;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }
            return null;
        }
    }
}
