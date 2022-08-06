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
                return PlayerName.FromString(value.ToString());
            }

            return value;
        }
    }
}
