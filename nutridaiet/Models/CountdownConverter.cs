using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace nutridaiet.Models;

public class CountdownConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // 处理空值
        if (value is not int intValue || intValue < 0)
            return "获取验证码";

        return intValue > 0 ? $"重新发送({intValue}s)" : "获取验证码";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException("单向绑定无需反向转换");
    }
}