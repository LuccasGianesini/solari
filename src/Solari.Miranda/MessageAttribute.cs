using System;

namespace Solari.Miranda
{
    /// <summary>
    /// FROM CONVEY
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageAttribute : Attribute
    {
        public string Durability { get; set; }
        public string Exchange { get; }
        public string RoutingKey { get; }
        public string Queue { get; }
        public bool External { get; }

        public MessageAttribute(string exchange = null, string routingKey = null, string queue = null,
                              bool external = false, string durability = "")
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
            External = external;
        }
    }
}