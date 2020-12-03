using System;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Contracts;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Extensions;

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
            Check.ThrowIfNull(url, nameof(MongoUrl), new CallistoException("Unable to create a 'MongoClientSettings' from a null 'MongoUrl' object."));
            return CreateMongoClientSettings(a => MongoClientSettings.FromUrl(url));
        }

        public static MongoClientSettings ConfigureTracing(this MongoClientSettings settings, bool trace,
                                                           ICallistoEventListener eventListener)
        {
            if (!trace)
                return settings;
            Check.ThrowIfNull(eventListener, nameof(ICallistoEventListener),
                              new CallistoException("An instance of 'ICallistoEventListener' must be provided to the method so that tracing can be configured for the connection."));
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
            Check.ThrowIfNull(settings, nameof(MongoClientSettings),
                              new CallistoException("The application is unable to create an instance of a 'IMongoClient' without a valid instance of 'MongoClientSettings'."));
            return new MongoClient(settings);
        }

        public static ICallistoClient CreateCallistoClient(this IMongoClient client)
        {
            Check.ThrowIfNull(client, nameof(IMongoClient), new CallistoException("'ICallistoClient' requires a valid instance of 'IMongoClient'."));
            return new CallistoClient(client);
        }

        public static MongoUrl CreateUrl(this CallistoConnectorOptions options, ApplicationOptions app)
        {
            var builder = new MongoUrlBuilder();
            builder.Parse(options.ConnectionString);
            if (Check.IsNull(app) || app.ApplicationName.IsNullOrEmpty())
            {
                builder.ApplicationName = options.ApplicationName;
            }
            else
            {
                builder.ApplicationName = app.ApplicationName;
            }

            builder.ConnectTimeout = options.ConnectTimeout.IsNullOrEmpty()
                                         ? MongoDefaults.ConnectTimeout
                                         : options.ConnectTimeout.ToTimeSpan();

            builder.MaxConnectionIdleTime = options.MaxConnectionIdleTime.IsNullOrEmpty()
                                                ? MongoDefaults.MaxConnectionIdleTime
                                                : options.MaxConnectionIdleTime.ToTimeSpan();

            builder.MaxConnectionLifeTime = options.MaxConnectionLifeTime.IsNullOrEmpty()
                                                ? MongoDefaults.MaxConnectionLifeTime
                                                : options.MaxConnectionLifeTime.ToTimeSpan();

            builder.MaxConnectionPoolSize =
                options.MaxConnectionPoolSize == 0 ? MongoDefaults.MaxConnectionPoolSize : options.MaxConnectionPoolSize;

            builder.MinConnectionPoolSize =
                options.MinConnectionPoolSize == 0 ? MongoDefaults.MinConnectionPoolSize : options.MinConnectionPoolSize;

            builder.HeartbeatInterval = options.HeartbeatInterval.IsNullOrEmpty()
                                            ? TimeSpan.FromSeconds(60)
                                            : options.HeartbeatInterval.ToTimeSpan();

            builder.HeartbeatTimeout = options.HeartbeatTimeout.IsNullOrEmpty()
                                           ? TimeSpan.FromSeconds(60)
                                           : options.HeartbeatTimeout.ToTimeSpan();

            builder.LocalThreshold = options.LocalThreshold.IsNullOrEmpty()
                                         ? MongoDefaults.LocalThreshold
                                         : options.LocalThreshold.ToTimeSpan();

            builder.RetryWrites = options.RetryWrites;

            builder.ServerSelectionTimeout = options.ServerSelectionTimeout.IsNullOrEmpty()
                                                 ? MongoDefaults.ServerSelectionTimeout
                                                 : options.ServerSelectionTimeout.ToTimeSpan();

            builder.SocketTimeout = options.SocketTimeout.IsNullOrEmpty()
                                        ? MongoDefaults.SocketTimeout
                                        : options.SocketTimeout.ToTimeSpan();


            builder.WaitQueueTimeout = options.WaitQueueTimeout.IsNullOrEmpty()
                                           ? MongoDefaults.WaitQueueTimeout
                                           : options.WaitQueueTimeout.ToTimeSpan();

            builder.IPv6 = options.Ipv6;

            return builder.ToMongoUrl();
        }
    }
}
