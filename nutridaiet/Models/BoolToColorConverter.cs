using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace nutridaiet.Models;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parameters = parameter?.ToString()?.Split('|');
        var trueColor = parameters?[0] ?? "#7ED321";
        var falseColor = parameters?[1] ?? "#CCCCCC";
        return (bool)value ? trueColor : falseColor;
    }

   
}

