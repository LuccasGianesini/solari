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
using Solari.Io;
using Solari.Sol;

namespace Solari.Hyperion
{
    public class HyperionStartupProcedure : IHostedService
    {
        private readonly IConsulClient _client;
        private readonly IServiceProvider _provider;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly HyperionOptions _options;
        private readonly ApplicationOptions _app;
        private CancellationTokenSource _cts;

        public HyperionStartupProcedure(IConsulClient client, IServiceProvider provider, IOptions<ApplicationOptions> app, IOptions<HyperionOptions> options,
                                        IHostApplicationLifetime applicationLifetime)
        {
            _client = client;
            _provider = provider;
            _applicationLifetime = applicationLifetime;
            _options = options.Value;
            _app = app.Value;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _applicationLifetime.ApplicationStarted.Register(OnStarted);
            _applicationLifetime.ApplicationStopping.Register(OnStopping);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            OnStopping();
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            AgentServiceRegistration registration = BuildServiceRegistration();
            OnStopping();
            HyperionLogger.HostedServiceLogger.LogRegisteringService(_app.ApplicationInstanceId);
            Task.Run(async () => await _client.Agent.ServiceRegister(registration, _cts.Token));
        }

        private void OnStopping()
        {
            HyperionLogger.HostedServiceLogger.LogDeregistering(_app.ApplicationId);
            try
            {
                Task.Run(async () => await _client.Agent.ServiceDeregister(_app.ApplicationId, _cts.Token));
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
            {
                // Throw when ASPNETCORE_URLS is not defined.
                throw new HyperionHostedException("Could not find instance host IP. ");
            }


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
            if (_options.AddHealthCheck)
            {
                return new AgentServiceCheck()
                {
                    HTTP = $"{appUri.Scheme}://{ip}:{appUri.Port}/hc",
                    Timeout = _options.CheckTimeout.ToTimeSpan(),
                    Interval = _options.CheckInterval.ToTimeSpan(),
                    TLSSkipVerify = _options.TlsSkipVerify,
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5)
                };
            }

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