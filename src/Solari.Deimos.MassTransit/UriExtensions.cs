using System;

namespace Solari.Deimos.MassTransit
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public static class UriExtensions
    {
        public static string GetExchangeName(this Uri value)
        {
            string exchange = value.LocalPath;
            string messageType = exchange.Substring(exchange.LastIndexOf('/') + 1);
            return messageType;
        }
    }
}
