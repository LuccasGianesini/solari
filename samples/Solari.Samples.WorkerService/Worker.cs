using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solari.Deimos.Abstractions;
using Solari.Samples.Application;
using Solari.Samples.Domain;
using Solari.Titan;

namespace Solari.Samples.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ITitanLogger<Worker> _logger;
        private readonly IMirandaPublisher _publisher;
        private readonly ITestClient _client;
        private readonly IDeimosTracer _tracer;


        public Worker(ITitanLogger<Worker> logger, IMirandaPublisher publisher, ITestClient client, IDeimosTracer tracer)
        {
            _logger = logger;
            _publisher = publisher;
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
                    // string val = await _client.Get();
                    await _publisher.PublishTestMessage(new TestMessage {Value = $"teste-{i}"});
                }

                i++;
            }
        }
    }
}