using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Solari.Deimos.MassTransit
{
    //COPIED FROM https://github.com/yesmarket/MassTransit.OpenTracing
    public class OpenTracingPipeSpecification : IPipeSpecification<ConsumeContext>, IPipeSpecification<PublishContext>
    {
        private readonly IConfiguration _configuration;

        public OpenTracingPipeSpecification(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<ConsumeContext> builder)
        {
            builder.AddFilter(new OpenTracingConsumeFilter(_configuration));
        }

        public void Apply(IPipeBuilder<PublishContext> builder)
        {
            builder.AddFilter(new OpenTracingPublishFilter(_configuration));
        }
    }
}
