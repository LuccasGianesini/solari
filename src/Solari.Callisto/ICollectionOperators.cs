using Solari.Callisto.Abstractions;
using Solari.Callisto.Framework.Operators;

namespace Solari.Callisto
{
    public interface ICollectionOperators<T> where T: class, IDocumentRoot
    {
         DeleteOperator<T> DeleteOperator { get; }
         ReplaceOperator<T> ReplaceOperator { get; }
         InsertOperator<T> InsertOperator { get; }
         QueryOperator<T> QueryOperator { get; }
         UpdateOperator<T> UpdateOperator { get; }

    }
}
