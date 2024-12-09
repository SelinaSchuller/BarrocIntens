using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;

namespace BarrocIntens.Converters
{
    //When a decimal value is passed, it will be formatted to 2 decimal places so like this: 0.00
    public class PriceDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal totalPrice)
            {
                return totalPrice.ToString("F2", CultureInfo.InvariantCulture);
            }
            else if (value is double totalPriceDouble)
            {
                return totalPriceDouble.ToString("F2", CultureInfo.InvariantCulture);
            }
            else if (value is float totalPriceFloat)
            {
                return totalPriceFloat.ToString("F2", CultureInfo.InvariantCulture);
            }
            else if (value is int totalPriceInt)
            {
                return totalPriceInt.ToString("F2", CultureInfo.InvariantCulture);
            }
            return "0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
            // Not used yet but is needed for IValueConverter
        }
    }
}