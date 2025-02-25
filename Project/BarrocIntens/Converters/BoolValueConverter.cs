﻿using Microsoft.UI.Xaml.Data;
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
                return boolValue == true ? "Ja" : "Nee";
            }
            return "Nee";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
            // Not used yet but is needed for IValueConverter
        }
    }
}