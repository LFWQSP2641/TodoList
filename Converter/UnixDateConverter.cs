using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace TodoList.Converter;

public class UnixDateConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long sec) return value?.ToString() ?? "";
        var dt = DateTimeOffset.FromUnixTimeSeconds(sec).ToLocalTime();
        var fmt = parameter as string ?? "yyyy-MM-dd HH:mm:ss";
        return dt.ToString(fmt, culture);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => BindingNotification.UnsetValue;
}