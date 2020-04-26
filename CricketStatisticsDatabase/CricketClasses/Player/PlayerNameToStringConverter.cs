﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Cricket.Player
{
    public class PlayerNameToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(PlayerName))
            {
                return value.ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var splitted = value.ToString().Split(" ");
                if (splitted.Length == 2)
                {
                    return new PlayerName(splitted[1], splitted[0]);
                }

                return new PlayerName();
            }

            return value;
        }
    }
}