using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WuxiaReader.Interface.Converters
{
    [ValueConversion(typeof(int), typeof(Thickness))]
    public class BottomMarginConverter : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ??= new BottomMarginConverter();
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int bottomMargin))
                return DependencyProperty.UnsetValue;
            
            return new Thickness(0, 0, 0, bottomMargin);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Thickness margin))
                return DependencyProperty.UnsetValue;

            return margin.Bottom;
        }
    }
}