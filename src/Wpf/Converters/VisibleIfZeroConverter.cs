using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Wpf.Converters;


[ValueConversion(typeof(int), typeof(Visibility))]
public class VisibleIfZeroConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            0 => Visibility.Visible,
            _ => Visibility.Hidden
        };


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
