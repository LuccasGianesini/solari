using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Io;

namespace Solari.Sol.Framework
{
    internal sealed class SolariMarshal : ISolariMarshal
    {
        private readonly ISolariBuilder _builder;
        private readonly ISolariPostConfigure _postConfigure;

        public SolariMarshal(ISolariBuilder builder)
        {
            _builder       = builder;
            _postConfigure = new SolariPostConfigure(this);
        }

        public IApplicationBuilder ApplicationBuilder { get; private set; }
        public IHost Host { get; private set; }

        public IServiceProvider Provider { get; private set; }

        
        public ISolariMarshal ExecuteBuildActions()
        {
            if (Provider == null)
                throw new Exception("IServiceProvider is null. Please build the service provider or set the value using the SetServiceProvider() method.");

            _builder.BuildActions.ExecuteAction(Provider);

            return this;
        }

        public TService GetService<TService>()
        {
            return Provider.GetService<TService>();
        }

        public ISolariMarshal PostConfigureServices(Action<ISolariPostConfigure> action)
        {
            action(_postConfigure);

            return this;
        }

        public ISolariMarshal PostConfigureServices(Action<IServiceProvider> action)
        {
            action(Provider);

            return this;
        }

        public ISolariMarshal SetApplicationBuilder(IApplicationBuilder applicationBuilder)
        {
            if (ApplicationBuilder != null) return this;

            ApplicationBuilder = applicationBuilder;

            return this;
        }

        public ISolariMarshal SetHost(IHost host)
        {
            if (Host != null) return this;

            Host = host;

            return this;
        }

        public ISolariMarshal SetServiceProvider(IServiceProvider serviceProvider)
        {
            if (Provider != null) return this;

            Provider = serviceProvider ?? throw new ArgumentNullException(nameof(Provider), "A null instance of IServiceProvider was provided.");

            return this;
        }
    }
}