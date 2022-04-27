using System;
using System.Globalization;
using System.Windows.Data;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace Wpf.Converters;


[ValueConversion(typeof(string), typeof(string))]
public class LocalizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var localizer = Bootstrapper.GlobalServiceProvider.GetRequiredService<ILocalizer>();

        return localizer[value.ToString()!];
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
