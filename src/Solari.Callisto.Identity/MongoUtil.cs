using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenTracing.Util;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Abstractions;
using Solari.Sol.Abstractions.Utils;

namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public static class MongoUtil
    {
        private static FindOptions<TItem> LimitOneOption<TItem>()
            => new FindOptions<TItem>
            {
                Limit = 1
            };


        public static IMongoCollection<TItem> FromCallistoConnectorOptions<TItem>(CallistoConnectorOptions options,
                                                                                  ApplicationOptions app,
                                                                                  string collectionName,
                                                                                  string database)
        {
            Type type = typeof(TItem);
            IMongoClient client = CreateFromOptions(options, app);
            return client.GetDatabase(database ?? "default").GetCollection<TItem>(collectionName ?? type.Name.ToLowerInvariant());
        }

        public static async Task<TItem> FirstOrDefaultAsync<TItem>(this IMongoCollection<TItem> collection, Expression<Func<TItem, bool>> p,
                                                                   CancellationToken cancellationToken = default)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return await (await collection.FindAsync(p, LimitOneOption<TItem>(), cancellationToken).ConfigureAwait(false)).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public static async Task<IEnumerable<TItem>> WhereAsync<TItem>(this IMongoCollection<TItem> collection, Expression<Func<TItem, bool>> p,
                                                                       CancellationToken cancellationToken = default)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return (await collection.FindAsync(p, cancellationToken: cancellationToken).ConfigureAwait(false)).ToEnumerable();
        }

        private static IMongoClient CreateFromOptions(CallistoConnectorOptions options, ApplicationOptions app)
        {
            CallistoJaegerEventListener eventListener = null;
            if (options.Trace)
            {
                if (GlobalTracer.Instance != null)
                {
                    eventListener = new CallistoJaegerEventListener(GlobalTracer.Instance, new EventFilter(), Options.Create(new CallistoTracerOptions()));
                }
            }

            MongoUrl url = options.CreateUrl(app);
            MongoClientSettings settings = url.CreateMongoClientSettings();
            if (eventListener != null)
                settings.ConfigureTracing(options.Trace, eventListener);
            return settings.CreateMongoClient();
        }
    }
}
