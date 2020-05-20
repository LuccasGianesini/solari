using System;

namespace Solari.Callisto.Abstractions
{
    public class RetrySpecification
    {
        public TimeSpan MinInterval { get; set; }
        public TimeSpan MaxInterval { get; set; }
        public int RetryLimit { get; set; }

        public TimeSpan GetDelta => MaxInterval - MinInterval;
    }
}