using Microsoft.Extensions.DependencyInjection;
using Solari.Sol;

namespace Solari.Eris
{
    public static class SolariBuilderExtensions
    {
        public static ISolariBuilder AddEris(this ISolariBuilder builder)
        {
            builder.Services.AddSingleton<IDispatcher, Dispatcher>();
            builder.Services.Scan(s =>
                                      s.FromAssemblies(builder.ApplicationAssemblies)
                                       .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                                       .AsImplementedInterfaces()
                                       .WithTransientLifetime()
                                       .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                                       .AsImplementedInterfaces()
                                       .WithTransientLifetime()
                                       .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                                       .AsImplementedInterfaces()
                                       .WithTransientLifetime()
                                       .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                                       .AsImplementedInterfaces()
                                       .WithTransientLifetime());

            return builder;
        }
    }
}