using System.Threading;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationContextHandler : ICorrelationContextHandler
    {
        private readonly AsyncLocal<ICorrelationContext> _current = new AsyncLocal<ICorrelationContext>();

        public ICorrelationContext Current
        {
            get => _current.Value;
            set => _current.Value = value;
        }
        
    }
}