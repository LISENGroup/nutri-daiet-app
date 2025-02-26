using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace nutridaiet.Models;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool)
        {
            return !(bool)value; // 反转布尔值
        }

        return value; // 如果不是布尔型，则直接返回原始值
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool)
        {
            return !(bool)value; // 反转布尔值
        }

        return value; // 如果不是布尔型，则直接返回原始值
    }
}