using App.Metrics;
using App.Metrics.Apdex;

namespace Solari.Ceres.Abstractions
{
    public class CeresMetrics
    {
        private readonly IMetrics _metrics;

        public CeresMetrics(IMetrics metrics) { _metrics = metrics; }
        
        public void MeasureException()
        {
            _metrics.Measure.Counter.Increment(MetricsRegistry.ErrorMetrics.CatchExceptionTotal);
            _metrics.Measure.Counter.Increment(MetricsRegistry.ErrorMetrics.CatchExceptionSinceLastReport);
        }
    }
}