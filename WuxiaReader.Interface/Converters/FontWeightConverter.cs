using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WuxiaReader.Interface.Converters
{
    [ValueConversion(typeof(int), typeof(FontWeight))]
    public class FontWeightConverter : MarkupExtension, IValueConverter
    {
        private static IValueConverter _instance;

        public static IValueConverter Instance => _instance ??= new FontWeightConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int weight))
                return DependencyProperty.UnsetValue;

            try
            {
                return FontWeight.FromOpenTypeWeight(weight);
            }
            catch
            {
                return FontWeights.Regular;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is FontWeight weight))
                return DependencyProperty.UnsetValue;

            return weight.ToOpenTypeWeight();
        }
    }
}