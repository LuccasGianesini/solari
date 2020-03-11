using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.CQR;
using Solari.Callisto.Abstractions.Exceptions;

namespace Solari.Callisto.Abstractions
{
    public static class CallistoOperationHelper
    {
        public static string NullDefinitionMessage(string op, string opName, string defType)
        {
            return $"The {op} {opName} has a null {defType} definition. The operation will not be carried out";
        }

        public static string NullOperationInstanceMessage(string op, string @interface) => $"The provided {@interface} {op} instance is null";
    }
}