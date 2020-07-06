using Solari.Callisto.Abstractions;
using Solari.Callisto.Framework;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public class CollectionOperators<T> : ICollectionOperators<T> where T: class, IDocumentRoot
    {
        public CollectionOperators(DeleteOperator<T> deleteOperator,
                                   ReplaceOperator<T> replaceOperator,
                                   InsertOperator<T> insertOperator,
                                   QueryOperator<T> queryOperator,
                                   UpdateOperator<T> updateOperator)
        {
            Delete = deleteOperator;
            Replace = replaceOperator;
            Insert = insertOperator;
            Query = queryOperator;
            Update = updateOperator;
        }

        public DeleteOperator<T> Delete { get; }
        public ReplaceOperator<T> Replace { get; }
        public InsertOperator<T> Insert { get; }
        public QueryOperator<T> Query { get; }
        public UpdateOperator<T> Update { get; }
    }
}
