using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace BarrocIntens.Converters
{
    // Converts a boolean value to a string value (true = Yes, False = No)
    public class BoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue == true ? "Yes" : "No";
            }
            return "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}