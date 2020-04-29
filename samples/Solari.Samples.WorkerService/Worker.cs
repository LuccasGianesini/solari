using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Solari.Deimos.Abstractions;
using Solari.Titan;

namespace Solari.Samples.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ITitanLogger<Worker> _logger;
        private readonly ITestClient _client;
        private readonly IDeimosTracer _tracer;


        public Worker(ITitanLogger<Worker> logger,ITestClient client, IDeimosTracer tracer)
        {
            _logger = logger;
            _client = client;
            _tracer = tracer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            long i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _tracer.OpenTracer.BuildSpan("Get-Github-Profile-Worker-Loop").StartActive(true))
                {
                    _logger.Information("Worker running at: {time}", DateTimeOffset.Now);
                }

                i++;
            }
        }
    }
}