using RawRabbit.Configuration.Exchange;

namespace Solari.Miranda.Options
{
    public class MirandaExchangeOptions
    {
        public bool AutoDelete { get; set; }
        public bool Durable { get; set; }
        public string Type { get; set; }

        public ExchangeType GetExchangeType()
        {
            return Type.ToLowerInvariant() switch
                   {
                       "topic"   => ExchangeType.Topic,
                       "direct"  => ExchangeType.Direct,
                       "fanout"  => ExchangeType.Fanout,
                       "headers" => ExchangeType.Headers,
                       "unkown" => ExchangeType.Unknown,
                       _         => ExchangeType.Topic
                   };
        }
    }
}