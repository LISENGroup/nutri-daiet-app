using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace nutridaiet.Models;

public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // 处理空值
        if (value is not bool boolValue)
            return "#CCCCCC"; // 默认颜色

        var parameters = parameter?.ToString()?.Split('|');
        var trueColor = parameters?[0] ?? "#7ED321";
        var falseColor = parameters?[1] ?? "#CCCCCC";

        return boolValue ? trueColor : falseColor;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // 不需要反向转换，直接抛出异常
        throw new NotSupportedException("单向绑定无需反向转换");
    }
}