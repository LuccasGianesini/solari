using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Solari.Io
{
    public static class StringExtensions
    {
        private static readonly Regex RxDigits = new Regex(@"[\d]+");

        /// <summary>
        ///     Removes all the non digit characters from a given string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>String without digits</returns>
        public static string CleanStringOfNonDigits(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var sb = new StringBuilder();
            for (Match m = RxDigits.Match(value); m.Success; m = m.NextMatch()) sb.Append(m.Value);

            return sb.ToString();
        }

        /// <summary>
        ///     Parses a string into a decimal.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Decimal value or 0 if parsing failed</returns>
        public static decimal ToDecimal(this string value)
        {
            return decimal.TryParse(CleanStringOfNonDigits(value), out decimal @decimal) ? @decimal : 0;
        }

        /// <summary>
        ///     Parses a string into a double.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Double value or 0 if parsing failed</returns>
        public static double ToDouble(this string value)
        {
            return double.TryParse(CleanStringOfNonDigits(value), out double @double) ? @double : 0;
        }

        /// <summary>
        ///     Parses a string into a float.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Float value or 0 if parsing failed</returns>
        public static float ToFloat(this string value)
        {
            return float.TryParse(CleanStringOfNonDigits(value), out float @float) ? @float : 0;

        }

        /// <summary>
        ///     Parses a string into a integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Integer value or 0 if parsing failed</returns>
        public static int ToInt(this string value)
        {
            return int.TryParse(CleanStringOfNonDigits(value), out int @int) ? @int : 0;
        }

        /// <summary>
        ///     Parses a string into a long.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Long value or 0 if parsing failed</returns>
        public static long ToLong(this string value)
        {
            return long.TryParse(CleanStringOfNonDigits(value), out long @long) ? @long : 0;
        }

        /// <summary>
        ///     Converts a string into a <see cref="TimeSpan"/>
        ///     <remark>
        ///         <code>
        ///             string value = "ms1000";
        ///             value.ToTimeSpan();
        ///         </code>
        ///         ms: Milliseconds.
        ///         s: Seconds.
        ///         m: Minutes.
        ///         h: Hour.
        ///         d: Day.
        ///         t: Ticks.
        ///     </remark>
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// Timespan value or <see cref="TimeSpan.MinValue"/>. If string is null, <see cref="TimeSpan.MinValue"/> will be returned.
        /// </returns>
        public static TimeSpan ToTimeSpan(this string value)
        {
            if(string.IsNullOrEmpty(value)) return TimeSpan.MinValue;
            string lowered = value.ToLowerInvariant();
            if (lowered.StartsWith("ms"))
                return TimeSpan.FromMilliseconds(ToLong(value));
            if (lowered.StartsWith("s"))
                return TimeSpan.FromMilliseconds(ToLong(value));
            if (lowered.StartsWith("m"))
                return TimeSpan.FromMilliseconds(ToLong(value));
            if (lowered.StartsWith("h"))
                return TimeSpan.FromMilliseconds(ToLong(value));
            if (lowered.StartsWith("t"))
                return TimeSpan.FromMilliseconds(ToLong(value));
            return lowered.StartsWith("d") ? TimeSpan.FromMilliseconds(ToLong(value)) : TimeSpan.MinValue;
        }
    }
}