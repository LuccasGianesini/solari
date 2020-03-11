using System;

namespace Solari.Callisto.Abstractions
{
    public static class DateTimeHelper
    {
        public static readonly DateTime DefaultDateTimeValue = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly DateTimeOffset DefaultDateTimeOffsetValue = new DateTimeOffset(DefaultDateTimeValue, TimeSpan.Zero);
        
        /// <summary>
        /// Create a utc <see cref="DateTimeOffset"/> based on the value provided.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static DateTimeOffset BuildDateTimeOffset(DateTimeOffset value)
        {
            DateTimeOffset? dateTimeOffset = new DateTimeOffset(new DateTime(value.Ticks, DateTimeKind.Utc), value.Offset);
            if (dateTimeOffset == DateTimeOffset.MinValue)
                dateTimeOffset = DefaultDateTimeOffsetValue;

            return (DateTimeOffset) dateTimeOffset;
        }

        /// <summary>
        /// Create a utc <see cref="DateTime"/> based on the value provided.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static DateTime BuildDateTimeValue(DateTime value)
        {
            DateTime? dateTime = new DateTime(value.Ticks, DateTimeKind.Utc);

            if (dateTime == DateTime.MinValue)
                dateTime = DefaultDateTimeValue;

            return (DateTime) dateTime;
        }
    }
}