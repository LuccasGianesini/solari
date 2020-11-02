using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Solari.Rhea.Tests
{
    public class TestFilterTwo : IRheaPipelineFilter
    {
        private readonly ILogger<TestFilterTwo> _logger;

        public TestFilterTwo(ILogger<TestFilterTwo> logger)
        {
            _logger = logger;
        }

        public Task Call(PipelineContext context)
        {
            _logger.LogInformation("Calling filter two");
            return Task.CompletedTask;
        }
    }
}
