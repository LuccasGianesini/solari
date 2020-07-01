using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Solari.Hyperion.Abstractions;
using Solari.Sol;
using Solari.Sol.Extensions;

namespace Solari.Hyperion
{
    public class HyperionStartupProcedure : IHostedService, IDisposable
    {
        private readonly ApplicationOptions _app;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IConsulClient _client;
        private readonly HyperionOptions _options;
        private readonly IServiceProvider _provider;
        private CancellationTokenSource _cts;
        private bool _disposed;

        public HyperionStartupProcedure(IConsulClient client, IServiceProvider provider, IOptions<ApplicationOptions> app,
                                        IOptions<HyperionOptions> options,
                                        IHostApplicationLifetime applicationLifetime)
        {
            _client = client;
            _provider = provider;
            _applicationLifetime = applicationLifetime;
            _options = options.Value;
            _app = app.Value;
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _cts?.Dispose();
            _disposed = true;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _applicationLifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    await OnStarted(cancellationToken);
                }
                catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    // Halt
                }
            });
            _applicationLifetime.ApplicationStopping.Register(async () =>
            {
                try
                {
                    await OnStopping(cancellationToken);
                }
                catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    await OnStopping(CancellationToken.None);
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }

        private async Task OnStarted(CancellationToken cancellationToken)
        {
            AgentServiceRegistration registration = BuildServiceRegistration();
            HyperionLogger.HostedServiceLogger.LogRegisteringService(_app.ApplicationInstanceId);
            await _client.Agent.ServiceRegister(registration, cancellationToken);
        }

        private async Task OnStopping(CancellationToken cancellationToken)
        {
            HyperionLogger.HostedServiceLogger.LogDeregistering(_app.ApplicationId);
            try
            {
                await _client.Agent.ServiceDeregister(_app.ApplicationId, cancellationToken);
            }
            catch (Exception ex)
            {
                HyperionLogger.HostedServiceLogger.LogDeregisterError(_app.ApplicationId, ex.Message);
            }
        }

        private AgentServiceRegistration BuildServiceRegistration()
        {
            string address = GetApplicationAddress();
            Uri appUri = GetApplicationUri(address);
            IPAddress ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            if (ip == null)
                // Throw when ASPNETCORE_URLS is not defined.
                throw new HyperionHostedException("Could not find instance host IP. ");


            return new AgentServiceRegistration
            {
                Name = _app.ApplicationName,
                Address = ip.ToString(),
                Port = appUri.Port,
                ID = _app.ApplicationId,
                Tags = BuildTags(),
                Check = BuildServiceCheck(appUri, ip)
            };
        }

        private AgentServiceCheck BuildServiceCheck(Uri appUri, IPAddress ip)
        {
            if (_options.HealthCheck is null)
                return default;

            if (_options.HealthCheck.AddHealthCheck)
                return new AgentServiceCheck
                {
                    HTTP = $"{appUri.Scheme}://{ip}:{appUri.Port}/hc",
                    Timeout = _options.HealthCheck.CheckTimeout.ToTimeSpan(),
                    Interval = _options.HealthCheck.CheckInterval.ToTimeSpan(),
                    TLSSkipVerify = _options.HealthCheck.TlsSkipVerify,
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5)
                };

            return default;
        }

        private string[] BuildTags()
        {
            _app.Tags.Add(_app.ApplicationEnvironment);
            return _app.Tags.ToArray();
        }

        private string GetApplicationAddress()
        {
            var server = _provider.GetService<IServer>();
            return server is null ? "localhost" : server.Features?.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
        }

        private Uri GetApplicationUri(string address) { return string.IsNullOrEmpty(address) ? null : new Uri(address.Replace("*", "localhost")); }
    }
}
