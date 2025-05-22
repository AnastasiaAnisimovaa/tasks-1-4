using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace CandyFactoryApp.Converters;

public class StringIsEqualConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string strValue && parameter is string paramValue)
        {
            return string.Equals(strValue, paramValue, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}