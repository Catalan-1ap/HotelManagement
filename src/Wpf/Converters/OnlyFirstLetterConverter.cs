using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;


namespace Wpf.Converters;


[ValueConversion(typeof(string), typeof(string))]
public class OnlyFirstLetterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            string s => s.FirstOrDefault(),
            _ => throw new ArgumentException($"{nameof(value)} expected type is string, but found {value.GetType()}")
        };


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
