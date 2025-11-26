using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace KeepExerciseA.Converters;

public class CountToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count && parameter is string conditionString &&
            int.TryParse(conditionString, out var condition))
        {
            return count > condition;
        }
        else return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}