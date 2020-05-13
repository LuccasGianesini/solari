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

namespace Solari.Ceres.Framework
{
    public class MemoryMeasurementHostedService : IHostedService, IDisposable
    {
        private const string Prefix = "Solari.Ceres (MemoryMeasurementHostedService):";
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<MemoryMeasurementHostedService> _logger;
        private readonly IMetrics _metrics;
        private readonly CeresOptions _options;
        private CancellationTokenSource _cts;
        private bool _disposed;
        private Task _executingTask;

        public MemoryMeasurementHostedService(IMetrics metrics, ILogger<MemoryMeasurementHostedService> logger,
                                              IOptions<CeresOptions> options, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _lifetime = lifetime;
            _options = options.Value;
            _metrics = metrics;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _cts.Dispose();
            _disposed = true;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(cancellationToken);
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts.Token.Register(() => _logger.LogDebug($"{Prefix}Cancellation requested. Stopping hosted service"));
            _lifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    await MeasureMemory(cancellationToken);
                }
                catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    // halt.
                }
            });

            return Task.CompletedTask;
        }

        private async Task MeasureMemory(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{Prefix}Starting hosted service");
            var process = Process.GetCurrentProcess();
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug($"{Prefix}Starting Memory Measurements");
                try
                {
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge,
                                                    () => process.PagedMemorySize64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge,
                                                    () => process.PeakPagedMemorySize64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge,
                                                    () => process.PeakVirtualMemorySize64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge,
                                                    () => process.WorkingSet64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge,
                                                    () => process.PrivateMemorySize64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge,
                                                    () => process.VirtualMemorySize64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge,
                                                    () => process.NonpagedSystemMemorySize64);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge,
                                                    () => process.PagedSystemMemorySize64);
                }
                catch (Exception e)
                {
                    _logger.LogError($"{Prefix}Error measuring memory consumption of the application", e);
                }

                _logger.LogDebug($"{Prefix}Awaiting next run.");
                await Task.Delay(_options.MemoryMetricsCollectorInterval.ToTimeSpan(), cancellationToken);
            }
        }
    }
}