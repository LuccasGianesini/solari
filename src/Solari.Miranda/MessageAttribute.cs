using System;

namespace Solari.Miranda
{
    /// <summary>
    /// FROM CONVEY
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageAttribute : Attribute
    {
        public string Exchange { get; }
        public string RoutingKey { get; }
        public string Queue { get; }
        public bool External { get; }

        public int Retries { get; }

        public double Interval { get; }


        public MessageAttribute(string exchange = null, string routingKey = null, string queue = null, bool external = false, int retries = 1,
                                double interval = 1)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
            External = external;
            Retries = retries;
            Interval = interval;
        }
    }
}