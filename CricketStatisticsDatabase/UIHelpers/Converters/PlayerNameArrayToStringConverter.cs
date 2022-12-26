using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using CricketStructures.Player;

namespace CricketStatisticsDatabase.UIHelpers.Converters
{
    public class PlayerNameArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if(value is List<PlayerName> array)
                {
                    string output = string.Empty;
                    for(int index = 0; index < array.Count; index++)
                    {
                        output +=array[index].ToString();
                        if(index < array.Count -1)
                        {
                            output += ",";
                        }
                    }

                    return output;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string[] names = value.ToString().Split(',');
                PlayerName[] output = new PlayerName[names.Length];
                for(int index = 0; index < names.Length;index++)
                {
                    string[] splitted = names[index].Split(' ');
                    if (splitted.Length == 2)
                    {
                        output[index] =  new PlayerName(splitted[1], splitted[0]);
                    }
                    if (splitted.Length == 1)
                    {
                        output[index] =  new PlayerName(splitted[0], "");
                    }
                }

                return output;
            }

            return value;
        }
    }
}
