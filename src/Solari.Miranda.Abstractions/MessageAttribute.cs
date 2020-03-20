using System;

namespace Solari.Miranda.Abstractions
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
        
        public string Name { get; }

        public MessageAttribute(string name = null,string exchange = null, string routingKey = null, string queue = null, bool external = false)
        {
            Exchange = exchange;
            RoutingKey = routingKey;
            Queue = queue;
            External = external;
        }
    }
}