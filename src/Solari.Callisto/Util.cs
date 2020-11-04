using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Solari.Callisto.Abstractions;
using Solari.Sol.Extensions;

namespace Solari.Callisto
{
    public static class Util
    {

        public static CallistoConnectorOptions GetCallistoConnectorOptions(this IEnumerable<CallistoConnectorOptions> clients, string clientName)
        {
            return clients.FirstOrDefault(a => a.Name.ToUpperInvariant().Equals(clientName.ToUpperInvariant()));
        }
        public static CallistoConnectorOptions GetCallistoConnectorOptions(this IConfiguration configuration, string clientName)
        {
            IConfigurationSection section = configuration.GetSection(CallistoConstants.ConnectorAppSettingsSection);
            var options = section.GetOptions<List<CallistoConnectorOptions>>();
            return options.GetCallistoConnectorOptions(clientName);
        }

        public static FindOptions<TItem> LimitOption<TItem>(int limit)
            => new FindOptions<TItem>
            {
                Limit = limit
            };

        public static FindOptions<TItem> LimitOneOption<TItem>()
            => new FindOptions<TItem>
            {
                Limit = 1
            };

        public static async Task<TItem> FirstOrDefaultAsync<TItem>(this IMongoCollection<TItem> collection,
                                                                   Expression<Func<TItem, bool>> p,
                                                                   CancellationToken cancellationToken = default)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return await (await collection.FindAsync(p, LimitOneOption<TItem>(), cancellationToken)
                                          .ConfigureAwait(false))
                         .FirstOrDefaultAsync(cancellationToken)
                         .ConfigureAwait(false);
        }

        public static async Task<IEnumerable<TItem>> WhereAsync<TItem>(this IMongoCollection<TItem> collection,
                                                                       Expression<Func<TItem, bool>> p,
                                                                       CancellationToken cancellationToken = default)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return (await collection.FindAsync(p, cancellationToken: cancellationToken)
                                    .ConfigureAwait(false))
                .ToEnumerable();
        }
    }
}
