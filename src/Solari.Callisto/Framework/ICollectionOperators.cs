using Solari.Callisto.Abstractions;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto.Framework
{
    public interface ICollectionOperators<T> where T: class, IDocumentRoot
    {
         DeleteOperator<T> Delete { get; }
         ReplaceOperator<T> Replace { get; }
         InsertOperator<T> Insert { get; }
         QueryOperator<T> Query { get; }
         UpdateOperator<T> Update { get; }

    }
}
