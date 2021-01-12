using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Solari.Rhea.Tests
{
    public class TestFilterOne : IRheaPipelineFilter
    {
        private readonly ILogger<TestFilterOne> _logger;
        private readonly TestFilterTwo _testFilterTwo;

        public TestFilterOne(ILogger<TestFilterOne> logger, TestFilterTwo testFilterTwo)
        {
            _logger = logger;
            _testFilterTwo = testFilterTwo;
        }
        public async Task Call(PipelineContext context)
        {
            _logger.LogInformation("Calling Filter One");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await _testFilterTwo.Call(context);

        }
    }
}
