using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Solari.Callisto
{
    public static class Util
    {
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
