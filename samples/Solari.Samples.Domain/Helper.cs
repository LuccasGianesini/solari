﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
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

        public static void DefaultCommandLogMessage<T>(ILogger<T> logger, string commandName, params string[] args) where T : class
        {
            StringBuilder builder;
            if (args != null && args.Length > 0)
            {
                builder = new StringBuilder();
                foreach (string arg in args)
                {
                    builder.Append($"Received '{commandName}' command").Append(arg).Append(" ");
                }

                logger.LogInformation(builder.ToString());
            }
            else
            {
                logger.LogInformation($"Received '{commandName}' command");
            }
        }

        public static void DefaultExceptionLogMessage<T>(ILogger<T> logger, MemberInfo info, Exception exception) where T : class
        {
            logger.LogError($"An exception of type {info.Name} was thrown. {exception.Message}", exception);
        }
    }
}