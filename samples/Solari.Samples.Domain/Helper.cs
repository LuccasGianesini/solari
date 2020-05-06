using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Solari.Titan;

namespace Solari.Samples.Domain
{
    public static class Helper
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = (MemberExpression) expression.Body;
            return body.Member.Name;
        }

        public static void DefaultCommandLogMessage<T>(ITitanLogger<T> logger, string commandName, params string[] args) where T : class
        {
            StringBuilder builder;
            if (args != null && args.Length > 0)
            {
                builder = new StringBuilder();
                foreach (string arg in args)
                {
                    builder.Append($"Received '{commandName}' command").Append(arg).Append(" ");
                }

                logger.Information(builder.ToString());
            }
            else
            {
                logger.Information($"Received '{commandName}' command");
            }
        }

        public static void DefaultExceptionLogMessage<T>(ITitanLogger<T> logger, MemberInfo info, Exception exception) where T : class
        {
            logger.Error($"An exception of type {info.Name} was thrown. {exception.Message}", exception);
        }
    }
}