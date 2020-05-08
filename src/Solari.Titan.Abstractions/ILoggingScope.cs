using System;

namespace Solari.Titan.Abstractions
{
    public interface ILoggingScope : IDisposable
    {
        void PushContext(IDisposable logContext);
        void CloseScope();

    }
}