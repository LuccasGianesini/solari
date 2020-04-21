namespace Solari.Deimos.CorrelationId.Framework
{
    public class CorrelationContextManager : ICorrelationContextManager
    {
        private readonly ICorrelationContextAccessor _accessor;
        private readonly ICorrelationContextFactory _factory;

        public CorrelationContextManager(ICorrelationContextAccessor accessor, ICorrelationContextFactory factory)
        {
            _accessor = accessor;
            _factory = factory;
        }

        public ICorrelationContext GetOrCreateAndSet(string messageId = "")
        {
            if (GetCurrent() != null)
                return GetCurrent();
            CreateAndSet(messageId);
            return GetCurrent();
        }

        public void CreateAndSet(string messageId = "")
        {
            ICorrelationContext context = _factory.CreateFromJaegerTracer(messageId);
            _accessor.Current = context;
        }

        public void Update(ICorrelationContext context) { _accessor.Current = context; }
        public ICorrelationContext GetCurrent() { return _accessor.Current; }
    }
}