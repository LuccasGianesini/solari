using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using Solari.Ganymede.Domain.Exceptions;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Extensions;

namespace Solari.Ganymede.Pipeline
{
    public class MessageStage : IPipelineStage
    {
        public MessageStage(PipelineContext pipelineContext)
        {
            PipelineContext = pipelineContext ?? throw new GanymedeException("Pipeline context instance is null");
        }

        /// <inheritdoc />
        public PipelineContext PipelineContext { get; }

        public static implicit operator PipelineContext(MessageStage messageStage) { return messageStage.PipelineContext; }


        /// <summary>
        ///     Use the <see cref="GanymedeRequestResource" /> object to set the necessary attribute values.
        /// </summary>
        /// <returns></returns>
        public MessageStage UseGanymedeEndpointOptions()
        {
            if (PipelineContext.Resource == null) return this;

            WithHttpVerb(PipelineContext.Resource.GetVerb());

            WithHttpVersion(PipelineContext.Resource.GetHttpVersion());

            WithTimeout(PipelineContext.Resource.GetTimeout());

            WithCompletionOption(PipelineContext.Resource.GetCompletionOption());

            return this;
        }

        /// <summary>
        ///     Set the cancellation token of the <see cref="HttpResponseMessage" />.
        /// </summary>
        /// <param name="cancellationToken">
        ///     <see cref="System.Threading.CancellationToken" />
        /// </param>
        public MessageStage WithCancellationToken(CancellationToken cancellationToken)
        {
            PipelineContext.RequestMessage.SetCancellationToken(cancellationToken);

            return this;
        }

        /// <summary>
        ///     Add <see cref="HttpCompletionOption" /> to the current <see cref="PipelineContext" />.
        /// </summary>
        /// <param name="completionOption">The completion option</param>
        public MessageStage WithCompletionOption(HttpCompletionOption completionOption)
        {
            if (!Enum.IsDefined(typeof(HttpCompletionOption), completionOption))
                throw new InvalidEnumArgumentException(nameof(completionOption), (int) completionOption, typeof(HttpCompletionOption));

            PipelineContext.RequestMessage.SetCompletionOption(completionOption);

            return this;
        }

        /// <summary>
        ///     Set the <see cref="HttpMethod" /> for the current <see cref="PipelineContext" />.
        /// </summary>
        /// <param name="httpMethod">Method od the request</param>
        public MessageStage WithHttpVerb(HttpMethod httpMethod)
        {
            if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));

            PipelineContext.RequestMessage.Method = httpMethod;

            return this;
        }

        /// <summary>
        ///     Set the message http version.
        /// </summary>
        /// <param name="version">
        ///     <see cref="System.Version" />
        /// </param>
        public MessageStage WithHttpVersion(Version version)
        {
            PipelineContext.RequestMessage.Version = version ?? throw new ArgumentNullException(nameof(version));

            return this;
        }


        /// <summary>
        ///     Set a timeout for the request.
        /// </summary>
        /// <param name="timeout"></param>
        public MessageStage WithTimeout(TimeSpan timeout)
        {
            PipelineContext.RequestMessage.SetTimeout(timeout);

            return this;
        }
    }
}