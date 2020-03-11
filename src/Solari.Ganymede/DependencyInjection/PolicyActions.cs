using System;
using Microsoft.Extensions.DependencyInjection;
using Solari.Ganymede.Framework;
using Solari.Sol;

namespace Solari.Ganymede.DependencyInjection
{
    public class PolicyActions
    {
        private readonly ISolariBuilder _builder;

        public PolicyActions(ISolariBuilder builder)
        {
            _builder = builder;
        }

        public ISolariBuilder ConfigureRegistry(Action<GanymedePolicyRegistry> configureRegistry)
        {
            _builder.AddBuildAction(provider =>
            {
                var registry = provider.GetService<GanymedePolicyRegistry>();
                configureRegistry(registry);
            });

            return _builder;
        }
    }
}