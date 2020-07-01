using Solari.Callisto.Abstractions;
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
            DeleteOperator = deleteOperator;
            ReplaceOperator = replaceOperator;
            InsertOperator = insertOperator;
            QueryOperator = queryOperator;
            UpdateOperator = updateOperator;
        }

        public DeleteOperator<T> DeleteOperator { get; }
        public ReplaceOperator<T> ReplaceOperator { get; }
        public InsertOperator<T> InsertOperator { get; }
        public QueryOperator<T> QueryOperator { get; }
        public UpdateOperator<T> UpdateOperator { get; }
    }
}
