namespace Solari.Deimos.CorrelationId
{
    public interface ICorrelationContextManager
    {
        ICorrelationContext GetOrCreateAndSet(string messageId = "");
        void CreateAndSet(string messageId = "");
        ICorrelationContext GetCurrent();
        void Update(ICorrelationContext context);
    }
}