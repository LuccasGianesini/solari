using System;

namespace Solari.Rhea.Synchronizer
{
    public interface IWriteToShared<T>
    {
        void Write(Action<T> writeAction);
    }
}