using System;
using System.Globalization;
using System.Windows.Data;
using CricketStructures.Player;

namespace CricketStatisticsDatabase.UIHelpers.Converters
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
                string[] splitted = value.ToString().Split(' ');
                if (splitted.Length == 2)
                {
                    return new PlayerName(splitted[1], splitted[0]);
                }
                if (splitted.Length == 1)
                {
                    return new PlayerName(splitted[0], "");
                }

                return new PlayerName();
            }

            return value;
        }
    }
}
