using System;
using System.Globalization;
using System.Windows.Data;

using CricketStructures.Match.Innings;

namespace CricketStatisticsDatabase.UIHelpers.Converters
{
    public class OverToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(Over))
            {
                return value.ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return Over.FromString(value.ToString());
            }

            return value;
        }
    }
}
