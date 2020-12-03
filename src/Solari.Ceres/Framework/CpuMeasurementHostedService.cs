using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solari.Ceres.Abstractions;
using Solari.Sol.Abstractions.Extensions;

namespace Solari.Ceres.Framework
{
    public class CpuMeasurementHostedService : IHostedService, IDisposable
    {
        private const string Prefix = "Solari.Ceres (CpuMeasurementHostedService):";
        public static DateTime StartTime = DateTime.UtcNow;
        private static TimeSpan _start;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<CpuMeasurementHostedService> _logger;
        private readonly IMetrics _metrics;
        private readonly CeresOptions _options;
        private CancellationTokenSource _cts;
        private bool _disposed;
        private Task _executingTask;

        public CpuMeasurementHostedService(IOptions<CeresOptions> ceresOptions, ILogger<CpuMeasurementHostedService> logger,
                                           IHostApplicationLifetime lifetime, IMetrics metrics)
        {
            _logger = logger;
            _lifetime = lifetime;
            _metrics = metrics;
            _options = ceresOptions.Value;
        }

        public double CpuUsageTotal { get; private set; }

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

        private Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts.Token.Register(() => _logger.LogDebug($"{Prefix}Cancellation requested. Stopping hosted service"));
            _start = Process.GetCurrentProcess().TotalProcessorTime;
            _lifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    await MeasureCpu(cancellationToken);
                }
                catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    // Halting.
                }
            });
            return Task.CompletedTask;
        }

        private async Task MeasureCpu(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"{Prefix}Starting hosted service");
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug($"{Prefix}Starting CPU Measurements");
                TimeSpan newCpuTime = Process.GetCurrentProcess().TotalProcessorTime - _start;
                CpuUsageTotal = newCpuTime.TotalSeconds / (Environment.ProcessorCount * DateTime.UtcNow.Subtract(StartTime).TotalSeconds);
                try
                {
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.CpuUsageTotal, CpuUsageTotal);
                    _logger.LogDebug("Successfully measure cpu time of the application");
                }
                catch (Exception e)
                {
                    _logger.LogError($"{Prefix}Error measuring cpu time of the application", e);
                }

                _logger.LogDebug($"{Prefix}Awaiting next run.");
                await Task.Delay(_options.CpuMetricsCollectorInterval.ToTimeSpan(), cancellationToken);
            }
        }
    }
}
