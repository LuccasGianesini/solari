using System;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Sol.Abstractions;

namespace Solari.Callisto
{
    public static class Helper
    {
        public static readonly DateTime DefaultDateTimeValue = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly DateTimeOffset DefaultDateTimeOffsetValue = new DateTimeOffset(DefaultDateTimeValue, TimeSpan.Zero);

        /// <summary>
        ///     Create a utc <see cref="DateTimeOffset" /> based on the value provided.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static DateTimeOffset BuildDateTimeOffset(DateTimeOffset value)
        {
            DateTimeOffset? dateTimeOffset = value;
            if (dateTimeOffset == DateTimeOffset.MinValue)
                dateTimeOffset = DefaultDateTimeOffsetValue;

            return (DateTimeOffset) dateTimeOffset;
        }

        /// <summary>
        ///     Create a utc <see cref="DateTime" /> based on the value provided.
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

        public static void PreExecutionCheck<T>(ICallistoOperation<T> operation)
            where T : class, IDocumentRoot
        {
            Check.ThrowIfNull(operation, nameof(ICallistoOperation<T>), new CallistoException("Please provide a valid instance of a 'ICallistoOperation'"));
            operation.Validate();
        }
    }
}
