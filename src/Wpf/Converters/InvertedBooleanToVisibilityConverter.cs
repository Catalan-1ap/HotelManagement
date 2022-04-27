using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Wpf.Converters;


[ValueConversion(typeof(bool), typeof(Visibility))]
public class InvertedBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            true => Visibility.Hidden,
            false => Visibility.Visible,
            _ => Visibility.Visible
        };


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
