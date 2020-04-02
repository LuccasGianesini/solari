using System.Threading;

namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private readonly AsyncLocal<ICorrelationContext> _current = new AsyncLocal<ICorrelationContext>();

        public ICorrelationContext Current
        {
            get => _current.Value;
            set => _current.Value = value;
        }
    }
}