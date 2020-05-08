using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Solari.Sol.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex RxDigits = new Regex(@"[\d]+");

        public static bool ContainsOnly(this string @string, string allowed) { return @string.All(c => allowed.Contains(c.ToString())); }

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
        public static decimal ToDecimal(this string value) { return decimal.TryParse(CleanStringOfNonDigits(value), out decimal @decimal) ? @decimal : 0; }

        /// <summary>
        ///     Parses a string into a double.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Double value or 0 if parsing failed</returns>
        public static double ToDouble(this string value) { return double.TryParse(CleanStringOfNonDigits(value), out double @double) ? @double : 0; }

        /// <summary>
        ///     Parses a string into a float.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Float value or 0 if parsing failed</returns>
        public static float ToFloat(this string value) { return float.TryParse(CleanStringOfNonDigits(value), out float @float) ? @float : 0; }

        /// <summary>
        ///     '
        ///     Parses a string into a integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Integer value or 0 if parsing failed</returns>
        public static int ToInt(this string value) { return int.TryParse(CleanStringOfNonDigits(value), out int @int) ? @int : 0; }

        /// <summary>
        ///     Parses a string into a long.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Long value or 0 if parsing failed</returns>
        public static long ToLong(this string value) { return long.TryParse(CleanStringOfNonDigits(value), out long @long) ? @long : 0; }

        public static byte ToByte(this string value)
        {
            byte result = default;
            if (!string.IsNullOrEmpty(value))
                byte.TryParse(value, out result);
            return result;
        }

        public static bool ToBool(this string value)
        {
            bool result = default;
            if (!string.IsNullOrEmpty(value))
                bool.TryParse(value, out result);
            return result;
        }

        public static char ToChar(this string value)
        {
            char result = default;
            if (!string.IsNullOrEmpty(value))
                char.TryParse(value, out result);
            return result;
        }

        public static sbyte ToSByte(this string value)
        {
            sbyte result = default;
            if (!string.IsNullOrEmpty(value))
                sbyte.TryParse(value, out result);
            return result;
        }


        public static bool ToBool(this char value)
        {
            bool result = default;
            if (!string.IsNullOrEmpty(value.ToString()))
                bool.TryParse(value.ToString(), out result);
            return result;
        }

        public static byte ToByte(this char value)
        {
            byte result = default;
            if (!string.IsNullOrEmpty(value.ToString()))
                byte.TryParse(value.ToString(), out result);
            return result;
        }

        public static sbyte ToSByte(this char value)
        {
            sbyte result = default;
            if (!string.IsNullOrEmpty(value.ToString()))
                sbyte.TryParse(value.ToString(), out result);
            return result;
        }

        public static short ToInt16(this char value)
        {
            short result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                short.TryParse(value.ToString(), out result);
            return result;
        }

        public static ushort ToUInt16(this char value)
        {
            ushort result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                ushort.TryParse(value.ToString(), out result);
            return result;
        }

        public static int ToInt32(this char value)
        {
            var result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                int.TryParse(value.ToString(), out result);
            return result;
        }

        public static uint ToUInt32(this char value)
        {
            uint result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                uint.TryParse(value.ToString(), out result);
            return result;
        }

        public static long ToInt64(this char value)
        {
            long result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                long.TryParse(value.ToString(), out result);
            return result;
        }

        public static ulong ToUInt64(this char value)
        {
            ulong result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                ulong.TryParse(value.ToString(), out result);
            return result;
        }

        public static float ToFloat(this char value)
        {
            float result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                float.TryParse(value.ToString(), out result);
            return result;
        }

        public static double ToDouble(this char value)
        {
            double result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                double.TryParse(value.ToString(), out result);
            return result;
        }

        public static decimal ToDecimal(this char value)
        {
            decimal result = 0;
            if (!string.IsNullOrEmpty(value.ToString()))
                decimal.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        ///     Converts a string into a <see cref="TimeSpan" />
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
        ///     Timespan value or <see cref="TimeSpan.MinValue" />. If string is null, <see cref="TimeSpan.MinValue" /> will be returned.
        /// </returns>
        public static TimeSpan ToTimeSpan(this string value)
        {
            if (string.IsNullOrEmpty(value)) return TimeSpan.MinValue;
            string lowered = value.ToLowerInvariant();
            if (lowered.StartsWith("ms"))
                return TimeSpan.FromMilliseconds(ToLong(value));
            if (lowered.StartsWith("s"))
                return TimeSpan.FromSeconds(ToLong(value));
            if (lowered.StartsWith("m"))
                return TimeSpan.FromMinutes(ToLong(value));
            if (lowered.StartsWith("h"))
                return TimeSpan.FromHours(ToLong(value));
            if (lowered.StartsWith("t"))
                return TimeSpan.FromTicks(ToLong(value));
            return lowered.StartsWith("d") ? TimeSpan.FromMilliseconds(ToLong(value)) : TimeSpan.MinValue;
        }

        public static string DashToLower(this string value) => value.Dash().ToLowerInvariant();
        public static string Dash(this string value) { return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())); }
    }
}