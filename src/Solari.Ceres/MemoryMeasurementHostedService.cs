using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solari.Ceres.Abstractions;
using Solari.Sol.Extensions;

namespace Solari.Ceres
{
    public class MemoryMeasurementHostedService : IHostedService, IDisposable
    {
        private const string Prefix = "Solari.Ceres (MemoryMeasurementHostedService):";
        private readonly IMetrics _metrics;
        private readonly ILogger<MemoryMeasurementHostedService> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly CeresOptions _options;
        private CancellationTokenSource _cts;
        private bool _disposed;

        public MemoryMeasurementHostedService(IMetrics metrics, ILogger<MemoryMeasurementHostedService> logger, IOptions<CeresOptions> options,
                                              IHostApplicationLifetime lifetime)
        {
            _metrics = metrics;
            _logger = logger;
            _lifetime = lifetime;
            _options = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _lifetime.ApplicationStarted.Register(MeasureMemory);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts.Token.Register(() => _logger.LogDebug($"{Prefix}Cancellation requested. Stopping hosted service"));
            return Task.CompletedTask;
        }

        private void MeasureMemory()
        {
            _logger.LogDebug($"{Prefix}Starting hosted service");
            var process = Process.GetCurrentProcess();
            while (!_cts.IsCancellationRequested)
            {
                _logger.LogDebug($"{Prefix}Starting Memory Measurements");
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge, () => process.PagedMemorySize64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge, () => process.PeakPagedMemorySize64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge,
                                                () => process.PeakVirtualMemorySize64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge, () => process.WorkingSet64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge, () => process.PrivateMemorySize64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge, () => process.VirtualMemorySize64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge, () => process.NonpagedSystemMemorySize64);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge, () => process.PagedSystemMemorySize64);
                _logger.LogDebug($"{Prefix}Awaiting next run.");
                Task.Delay(_options.Memory.Interval.ToTimeSpan()).Wait();
            }
        }


        public Task StopAsync(CancellationToken cancellationToken) { return Task.CompletedTask; }

        public void Dispose()
        {
            if (_disposed)
                return;
            _cts.Dispose();
            _disposed = true;
        }
    }
}