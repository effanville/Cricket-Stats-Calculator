using System;

namespace StructureCommon.Extensions
{
    /// <summary>
    /// Static extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert a string into the enum type given by T.
        /// </summary>
        public static T ToEnum<T>(this string value)
        {
            T output = default(T);
            output = (T)Enum.Parse(typeof(T), value, true);
            return output;
        }

        /// <summary>
        /// Outputs a date in the UK format (the good format) from a datetime.
        /// </summary>
        public static string ToUkDateString(this DateTime date)
        {
            return date.Day + "/" + date.Month + "/" + date.Year;
        }
    }
}
