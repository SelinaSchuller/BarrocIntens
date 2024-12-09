using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace BarrocIntens.Converters
{
    // Seperates DateTime into a date string with a european format (dd-MM-yyyy)
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }
        // Converts a date string with a european format (dd-MM-yyyy) to a DateTime object (Note The time will be 00:00:00)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return null;
        }
    }
}