using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solari.Sol.Abstractions;
using Solari.Sol.Framework.Exceptions;

namespace Solari.Sol.Framework
{
    internal sealed class SolariMarshal : ISolariMarshal
    {
        private readonly ISolariBuilder _builder;
        private readonly ISolariPostConfigure _postConfigure;

        public SolariMarshal(ISolariBuilder builder)
        {
            _builder = builder;
            _postConfigure = new SolariPostConfigure(this);
        }

        public IApplicationBuilder ApplicationBuilder { get; private set; }
        public IHost Host { get; private set; }

        public IServiceProvider Provider { get; private set; }


        public ISolariMarshal ExecuteBuildActions()
        {
            if (Provider == null)
                throw new Exception("IServiceProvider is null. Please build the service provider or set the value using the ConfigureApplication() method.");

            foreach (BuildAction action in _builder.BuildActions)
            {
                SolLogger.MarshalLogger.ExecutingBuildAction(action.Name);
                action.Action.Invoke(Provider);
            }

            return this;
        }

        public ISolariMarshal ExecuteBuildAsyncActions()
        {
            foreach (BuildAction builderBuildAction in _builder.BuildActions) Task.Run(async () => await builderBuildAction.AsyncAction);
            return this;
        }

        public TService GetService<TService>() { return Provider.GetService<TService>(); }

        public ISolariMarshal PostConfigureServices(Action<ISolariPostConfigure> action)
        {
            action(_postConfigure);

            return this;
        }

        public ISolariMarshal ConfigureApplication(IServiceProvider provider, IApplicationBuilder applicationBuilder, IHost host)
        {
            Provider = provider ?? throw new NullServiceProviderException();
            if (applicationBuilder != null)
            {
                ApplicationBuilder = applicationBuilder;
                return this;
            }

            if (host == null) return this;
            Host = host;
            return this;

            ;
        }

        public ISolariMarshal PostConfigureServices(Action<IServiceProvider> action)
        {
            action(Provider);

            return this;
        }
    }
}
