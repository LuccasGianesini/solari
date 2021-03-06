﻿namespace Solari.Ganymede.Domain.Options
{
    public class GanymedeCircuitBreakerOptions
    {
        /// <summary>
        ///     Duration of the pause
        /// </summary>
        public string Duration { get; set; } = "s10";

        /// <summary>
        ///     Maximum number of exceptions accepted before break.
        /// </summary>
        public int NumberOfExceptionsBeforeBreaking { get; set; } = 3;
    }
}
