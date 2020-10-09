using System;
using System.Threading;
using Xunit;

namespace Solari.Rhea.Tests
{
    public class UnitTest1
    {
        class InputContext : IRailContext
        {

            public CancellationToken CancellationToken { get; }
            public int value { get; }
        }

        class OutputContext : IRailContext
        {
            public OutputContext(string value)
            {
                Value = value;
            }
            public CancellationToken CancellationToken { get; }
            public string Value { get; }
        }

        [Fact]
        public void Should_convert_a_context()
        {
            RailContextConverterFactory<InputContext, OutputContext> factory = input => new OutputContext("a");
            factory.Invoke(new InputContext());
        }


    }
}
