using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenTracing.Util;
using Solari.Callisto.Abstractions;
using Solari.Callisto.Abstractions.Exceptions;
using Solari.Callisto.Connector;
using Solari.Callisto.DependencyInjection;
using Solari.Callisto.Framework;
using Solari.Callisto.Tracer;
using Solari.Callisto.Tracer.Framework;
using Solari.Sol;
using Solari.Sol.Extensions;
using Solari.Sol.Utils;

namespace Solari.Callisto.Identity
{
    // COPIED FROM https://github.com/matteofabbri/AspNetCore.Identity.Mongo
    public static class DependencyInjection
    {
        public static ISolariBuilder AddCallistoIdentityProvider(this ISolariBuilder builder,
                                                                 string clientName,
                                                                 string database)
        {
            return builder.AddCallistoIdentityProvider(clientName, database, null);
        }

        public static ISolariBuilder AddCallistoIdentityProvider(this ISolariBuilder builder,
                                                                 string clientName,
                                                                 string database,
                                                                 Action<IdentityOptions> setupIdentity)
        {
            return builder.AddCallistoIdentityProvider<MongoUser, MongoRole>(opt =>
            {
                opt.ClientName = clientName;
                opt.Database = database;
            }, setupIdentity);
        }

        public static ISolariBuilder AddCallistoIdentityProvider(this ISolariBuilder builder,
                                                                 Action<CallistoIdentityOptions> setupOptions,
                                                                 Action<IdentityOptions> setupIdentity)
        {
            var opt = new CallistoIdentityOptions();
            setupOptions(opt);

            return builder.AddCallistoIdentityProvider<MongoUser, MongoRole>(opt.ClientName,
                                                                             opt.Database,
                                                                             opt.UsersCollection,
                                                                             opt.RolesCollection,
                                                                             setupIdentity);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser>(this ISolariBuilder builder,
                                                                        string clientName,
                                                                        string database)
            where TUser : MongoUser
        {
            return builder.AddCallistoIdentityProvider<TUser, MongoRole>(clientName, database, null);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser>(this ISolariBuilder builder,
                                                                        string clientName,
                                                                        string database,
                                                                        Action<IdentityOptions> setupIdentity)
            where TUser : MongoUser
        {
            return builder.AddCallistoIdentityProvider<TUser, MongoRole>(opt =>
            {
                opt.ClientName = clientName;
                opt.Database = database;
            }, setupIdentity);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser>(this ISolariBuilder builder,
                                                                        Action<CallistoIdentityOptions>
                                                                            setupOptions,
                                                                        Action<IdentityOptions> setupIdentity)
            where TUser : MongoUser
        {
            var opt = new CallistoIdentityOptions();
            setupOptions(opt);

            return builder.AddCallistoIdentityProvider<TUser, MongoRole>(opt.ClientName, opt.Database,
                                                                         opt.UsersCollection,
                                                                         opt.RolesCollection, setupIdentity);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser, TRole>(this ISolariBuilder builder,
                                                                               string clientName,
                                                                               string database)
            where TUser : MongoUser
            where TRole : MongoRole
        {
            return builder.AddCallistoIdentityProvider<TUser, TRole>(clientName, database, null);
        }

        public static ISolariBuilder AddCallistoIdentityProvider<TUser, TRole>(this ISolariBuilder builder,
                                                                               string clientName,
                                                                               string database,
                                                                               Action<IdentityOptions> setupIdentity)
            where TUser : MongoUser
            where TRole : MongoRole
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
            where TUser : MongoUser
            where TRole : MongoRole
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
            where TUser : MongoUser
            where TRole : MongoRole
        {
            IdentityBuilder identityBuilder = builder.Services.AddIdentity<TUser, TRole>(setupIdentity ?? (x => { }));

            identityBuilder.AddRoleStore<RoleStore<TRole>>()
                           .AddUserStore<UserStore<TUser, TRole>>()
                           .AddUserManager<UserManager<TUser>>()
                           .AddRoleManager<RoleManager<TRole>>()
                           .AddDefaultTokenProviders();


            var conventions = new CallistoConventionRegistry();
            conventions.AddDefaultConventions();
            conventions.RegisterConventionPack("CallistoIdentityConventionPack", type =>
            {
                bool user = typeof(TUser).IsAssignableFrom(typeof(MongoUser)) && typeof(TUser) == type;
                bool role = typeof(TRole).IsAssignableFrom(typeof(MongoRole)) && typeof(TRole) == type;
                return user && role;
            });
            ApplicationOptions app = builder.GetAppOptions();
            CallistoConnectorOptions options = builder.Configuration.GetCallistoConnectorOptions(clientName);
            IMongoCollection<TUser> users = MongoUtil.FromCallistoConnectorOptions<TUser>(options, app, usersCollection, database);
            IMongoCollection<TRole> roles = MongoUtil.FromCallistoConnectorOptions<TRole>(options, app, rolesCollection, database);
            builder.Services.AddSingleton(x => users);
            builder.Services.AddSingleton(x => roles);

            // Identity Services
            builder.Services.AddTransient<IRoleStore<TRole>>(x => new RoleStore<TRole>(roles));
            builder.Services.AddTransient<IUserStore<TUser>>(x => new UserStore<TUser, TRole>(users, new RoleStore<TRole>(roles), x.GetService<ILookupNormalizer>()));


            return builder;
        }

        private static MongoUrl CreateUrl(CallistoConnectorOptions options, ApplicationOptions app)
        {
            MongoUrl url = options.CreateUrl(app);
            return url;
        }

    }
}
