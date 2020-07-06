using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Callisto.Connector
{
    public static class CallistoConnectionFactory
    {
        public static MongoUrl CreateMongoUrl(Func<MongoUrlBuilder, MongoUrl> factory)
        {
            return factory(new MongoUrlBuilder());
        }
        public static MongoClientSettings CreateMongoClientSettings(Func<MongoClientSettings, MongoClientSettings> factory)
        {
            return factory(new MongoClientSettings());
        }
        public static MongoClientSettings CreateMongoClientSettings(this MongoUrl url)
        {
            if (url is null)
                throw new CallistoException("Unable to create a 'MongoClientSettings' from a null 'MongoUrl' object.");
            return CreateMongoClientSettings(a => MongoClientSettings.FromUrl(url));
        }
        public static MongoClientSettings ConfigureTracing(this MongoClientSettings settings, bool trace,
                                                           ICallistoEventListener eventListener)
        {
            if (!trace)
                return settings;
            if (eventListener is null)
                throw new
                    CallistoException("An instance of 'ICallistoEventListener' must be provided to the method so that " +
                                      "we can configure the tracing for the connection. ");

            settings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(eventListener.StartEventHandler)
                       .Subscribe<CommandSucceededEvent>(eventListener.SuccessEventHandler)
                       .Subscribe<CommandFailedEvent>(eventListener.ErrorEventHandler);
            };
            return settings;
        }
        public static IMongoClient CreateMongoClient(this MongoClientSettings settings)
        {
            if(settings is null)
                throw new CallistoException("The application is unable to create an instance of a 'IMongoClient' without " +
                                            "a valid instance of 'MongoClientSettings'. ");
            return new MongoClient(settings);
        }
        public static ICallistoClient CreateCallistoClient(this IMongoClient client)
        {
            if(client is null)
                throw new CallistoException("'ICallistoClient' requires a valid instance of 'IMongoClient'.");

            return new CallistoClient(client);
        }
        public static MongoUrl CreateUrl(this CallistoConnectorOptions options, ApplicationOptions app)
        {
            var builder = new MongoUrlBuilder();
            builder.Parse(options.ConnectionString);
            if (app is null || string.IsNullOrEmpty(app.ApplicationName))
            {
                builder.ApplicationName = options.ApplicationName;
            }
            else
            {
                builder.ApplicationName = app.ApplicationName;
            }

            builder.ConnectTimeout =
                string.IsNullOrEmpty(options.ConnectTimeout) ? MongoDefaults.ConnectTimeout : options.ConnectTimeout.ToTimeSpan();

            builder.MaxConnectionIdleTime = string.IsNullOrEmpty(options.MaxConnectionIdleTime)
                                                ? MongoDefaults.MaxConnectionIdleTime
                                                : options.MaxConnectionIdleTime.ToTimeSpan();

            builder.MaxConnectionLifeTime = string.IsNullOrEmpty(options.MaxConnectionLifeTime)
                                                ? MongoDefaults.MaxConnectionLifeTime
                                                : options.MaxConnectionLifeTime.ToTimeSpan();

            builder.MaxConnectionPoolSize =
                options.MaxConnectionPoolSize == 0 ? MongoDefaults.MaxConnectionPoolSize : options.MaxConnectionPoolSize;

            builder.MinConnectionPoolSize =
                options.MinConnectionPoolSize == 0 ? MongoDefaults.MinConnectionPoolSize : options.MinConnectionPoolSize;

            builder.HeartbeatInterval = string.IsNullOrEmpty(options.HeartbeatInterval)
                                            ? TimeSpan.FromSeconds(60)
                                            : options.HeartbeatInterval.ToTimeSpan();

            builder.HeartbeatTimeout =
                string.IsNullOrEmpty(options.HeartbeatTimeout) ? TimeSpan.FromSeconds(60) : options.HeartbeatTimeout.ToTimeSpan();

            builder.LocalThreshold =
                string.IsNullOrEmpty(options.LocalThreshold) ? MongoDefaults.LocalThreshold : options.LocalThreshold.ToTimeSpan();

            builder.RetryWrites = options.RetryWrites;

            builder.ServerSelectionTimeout = string.IsNullOrEmpty(options.ServerSelectionTimeout)
                                                 ? MongoDefaults.ServerSelectionTimeout
                                                 : options.ServerSelectionTimeout.ToTimeSpan();

            builder.SocketTimeout =
                string.IsNullOrEmpty(options.SocketTimeout) ? MongoDefaults.SocketTimeout : options.SocketTimeout.ToTimeSpan();


            builder.WaitQueueTimeout = string.IsNullOrEmpty(options.WaitQueueTimeout)
                                           ? MongoDefaults.WaitQueueTimeout
                                           : options.WaitQueueTimeout.ToTimeSpan();

            builder.IPv6 = options.Ipv6;

            return builder.ToMongoUrl();
        }
    }
}
