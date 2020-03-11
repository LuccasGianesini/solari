using System;

namespace Solari.Rhea.Synchronizer
{
    public interface IReadFromShared<TIn, TOut>
    {
        TOut Read(Func<TIn, TOut> readFunction);
    }
}