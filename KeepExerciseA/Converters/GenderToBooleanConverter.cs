using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
namespace KeepExerciseA.Converters
{
    public class GenderToBooleanConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string gender && parameter is string targetGender)
            {
                return gender == targetGender;
            }
            return false;
        }

       
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            
            if (value is bool isChecked && isChecked && parameter is string targetGender)
            {
                return targetGender;
            }
            return BindingOperations.DoNothing;
        }
    }
}