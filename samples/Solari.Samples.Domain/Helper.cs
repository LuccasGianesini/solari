using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solari.Samples.Domain.Person.Results;
using Solari.Titan;

namespace Solari.Samples.Domain
{
    public static class Helper
    {
        public static void DefaultCommandLogMessage<T>(ITitanLogger<T> logger, string commandName) where T : class
        {
            logger.Information($"Received {commandName} command");
        }

        public static void DefaultExceptionLogMessage<T>(ITitanLogger<T> logger, MemberInfo info, Exception exception) where T : class
        {
            logger.Error($"An exception of type {info.Name} was thrown. {exception.Message}", exception);
        }
    }
}