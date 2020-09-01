// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Solari.Ganymede.Domain.Options
{
    public class GanymedeRetryOptions
    {
        /// <summary>
        ///     Multiplier for the Delay
        /// </summary>
        public int BackOff { get; set; } = 5;

        /// <summary>
        ///     Retries count
        /// </summary>
        public int Count { get; set; } = 3;
    }
}
