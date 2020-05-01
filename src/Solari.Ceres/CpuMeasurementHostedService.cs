using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    public class CpuMeasurementHostedService : IHostedService, IDisposable
    {
        private const string Prefix = "Solari.Ceres (CpuMeasurementHostedService):";
        private readonly ILogger<CpuMeasurementHostedService> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IMetrics _metrics;
        private CancellationTokenSource _cts;
        private bool _disposed;
        private readonly CeresOptions _options;
        public static DateTime StartTime = DateTime.UtcNow;
        private static TimeSpan _start;
        private Process _process;
        public double CpuUsageTotal { get; private set; }
        
        public CpuMeasurementHostedService(IOptions<CeresOptions> ceresOptions, ILogger<CpuMeasurementHostedService> logger, 
                                           IHostApplicationLifetime lifetime, IMetrics metrics)
        {
            _logger = logger;
            _lifetime = lifetime;
            _metrics = metrics;
            _options = ceresOptions.Value;
            _process = Process.GetCurrentProcess();
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts.Token.Register(() => _logger.LogDebug($"{Prefix}Cancellation requested. Stopping hosted service"));
            _lifetime.ApplicationStarted.Register(MeasureCpu);
            _start = _process.TotalProcessorTime;

        }

        private void MeasureCpu()
        {
            _logger.LogDebug($"{Prefix}Starting hosted service");
            
            
            while (!_cts.IsCancellationRequested)
            {
                _logger.LogDebug($"{Prefix}Starting CPU Measurements");
                TimeSpan newCpuTime = _process.TotalProcessorTime - _start;
                CpuUsageTotal = newCpuTime.TotalSeconds / (Environment.ProcessorCount * DateTime.UtcNow.Subtract(StartTime).TotalSeconds);
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ProcessMetrics.CpuUsageTotal, CpuUsageTotal);
                
                _logger.LogDebug($"{Prefix}Awaiting next run.");
                Task.Delay(_options.Cpu.Interval.ToTimeSpan()).Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _cts.Dispose();
            _disposed = true;
        }
    }
}