using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WuxiaReader.Interface.Converters
{
    [ValueConversion(typeof(bool), typeof(FontStyle))]
    public class ItalicConverter : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ??= new ItalicConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool isItalic))
                return DependencyProperty.UnsetValue;

            return !isItalic ? FontStyles.Normal : FontStyles.Italic;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is FontStyle fontStyle))
                return DependencyProperty.UnsetValue;

            return fontStyle == FontStyles.Italic;
        }
    }
}