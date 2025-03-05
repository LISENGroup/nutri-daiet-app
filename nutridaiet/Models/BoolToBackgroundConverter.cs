using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace nutridaiet.Models;

public class BoolToBackgroundConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isSelected)
        {
            return isSelected ? new SolidColorBrush(Color.Parse("#3066FF00")) : new SolidColorBrush(Colors.Transparent);
        }
        return new SolidColorBrush(Colors.Transparent);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    
}