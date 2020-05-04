using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WuxiaReader.Interface.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    public class SubOneConverter : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ??= new SubOneConverter();
        
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int baseValue))
                return DependencyProperty.UnsetValue;

            return baseValue - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int baseValue))
                return DependencyProperty.UnsetValue;

            return baseValue + 1;
        }
    }
}