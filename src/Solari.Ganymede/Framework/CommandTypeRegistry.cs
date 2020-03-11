using System.Collections.Concurrent;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Framework.Commands;

namespace Solari.Ganymede.Framework
{
    public sealed class CommandTypeRegistry
    {
        
        private readonly ConcurrentDictionary<string, IHeaderBuilderCommand> _types = new ConcurrentDictionary<string, IHeaderBuilderCommand>
        {
            ["ACCEPT"] = new AcceptCommand(),
            ["ACCEPT-CHARSET"] = new AcceptCharsetCommand(),
            ["ACCEPT-ENCODING"] = new AcceptEncodingCommand(),
            ["ACCEPT-LANGUAGE"] = new AcceptLanguageCommand(),
            ["AUTHORIZATION"] = new AuthorizationCommand(),
            ["CACHE-CONTROL"] = new CacheControlCommand(),
            ["EXPECT"] = new ExpectCommand(),
            ["FROM"] = new FromCommand(),
            ["HOST"] = new HostCommand(),
            ["IF-MODIFIED-SINCE"] = new IfModifiedSinceCommand(),
            ["IF-UNMODIFIED-SINCE"] = new IfUnmodifiedSinceCommand(),
            ["PROXY-AUTHORIZATION"] = new ProxyAuthorizationCommand(),
            ["RANGE"] = new RangeCommand(),
            ["REFERRER"] = new ReferrerCommand(),
            ["USER-AGENT"] = new UserAgentCommand(),
            ["CUSTOM"] = new CustomHeaderCommand()
        };

        private static readonly CommandTypeRegistry SingletonInstance = new CommandTypeRegistry();

        static CommandTypeRegistry()
        {
        }

        private CommandTypeRegistry()
        {
        }

        // ReSharper disable once ConvertToAutoProperty
        public static CommandTypeRegistry Instance => SingletonInstance;

        public IHeaderBuilderCommand TryGetCommandType(string key)
        {
            if (_types.TryGetValue(key, out IHeaderBuilderCommand value)) return value;

            throw new CommandNotAvailableException();
        }
    }
}