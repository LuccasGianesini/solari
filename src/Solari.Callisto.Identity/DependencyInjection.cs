using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Solari.Callisto.Abstractions;
using Solari.Callisto.DependencyInjection;
using Solari.Sol;

namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public static class DependencyInjection
    {
        public static ISolariBuilder AddCallistoIdentityProvider<TUser, TRole>(this ISolariBuilder builder,
                                                                               string clientName,
                                                                               string database,
                                                                               Action<IdentityOptions> setupIdentity)
            where TUser : CallistoIdentityUser, IDocumentRoot
            where TRole : CallistoIdentityRole, IDocumentRoot
        {
            return builder.AddCallistoIdentityProvider<TUser, TRole>(opt =>
            {
                opt.ClientName = clientName;
                opt.Database = database;
            }, setupIdentity);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser, TRole>(this ISolariBuilder builder,
                                                                               Action<CallistoIdentityOptions>
                                                                                   setupOptions,
                                                                               Action<IdentityOptions> setupIdentity)
            where TUser : CallistoIdentityUser, IDocumentRoot
            where TRole : CallistoIdentityRole, IDocumentRoot
        {
            var opt = new CallistoIdentityOptions();
            setupOptions(opt);

            return builder.AddCallistoIdentityProvider<TUser, TRole>(opt.ClientName, opt.Database, opt.UsersCollection,
                                                                     opt.RolesCollection, setupIdentity);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser, TRole>(this ISolariBuilder builder,
                                                                               string clientName,
                                                                               string database,
                                                                               string usersCollection,
                                                                               string rolesCollection,
                                                                               Action<IdentityOptions> setupIdentity)
            where TUser : CallistoIdentityUser, IDocumentRoot
            where TRole : CallistoIdentityRole, IDocumentRoot
        {
            IdentityBuilder identityBuilder = builder.Services.AddIdentity<TUser, TRole>(setupIdentity ?? (x => { }));

            identityBuilder.AddRoleStore<CallistoRoleStore<TRole>>()
                           .AddUserStore<CallistoUserStore<TUser, TRole>>()
                           .AddUserManager<TUser>()
                           .AddRoleManager<TRole>()
                           .AddDefaultTokenProviders();

            var configurator = new CallistoCollectionConfigurator(builder, clientName);

            configurator.ConfigureTransientCollection<CallistoUserStore<TUser, TRole>,
                CallistoUserStore<TUser, TRole>, TUser>(database, usersCollection);
            configurator.ConfigureTransientCollection<CallistoRoleStore<TRole>,
                CallistoRoleStore<TRole>, TRole>(database, rolesCollection);

            return builder;
        }
    }
}
