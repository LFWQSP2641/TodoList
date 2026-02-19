using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using TodoList.Models;

namespace TodoList.Converter;

public class TodoLevelDisplayConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not TodoLevel level) return value?.ToString() ?? "";
        return level switch
        {
            TodoLevel.Low => "低",
            TodoLevel.Medium => "中",
            TodoLevel.High => "高",
            _ => level.ToString()
        };
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => BindingNotification.UnsetValue;
}