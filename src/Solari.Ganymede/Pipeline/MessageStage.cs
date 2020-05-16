using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Extensions;

namespace Solari.Ganymede.Pipeline
{
    public class MessageStage : IPipelineStage
    {
        public MessageStage(PipelineDescriptor pipelineDescriptor)
        {
            //TODO Create custom exceptions.
            PipelineDescriptor = pipelineDescriptor ?? throw new ArgumentNullException(nameof(pipelineDescriptor));
        }

        /// <inheritdoc />
        public PipelineDescriptor PipelineDescriptor { get; }

        public static implicit operator PipelineDescriptor(MessageStage messageStage) { return messageStage.PipelineDescriptor; }


        /// <summary>
        ///     Use the <see cref="GanymedeRequestResource" /> object to set the necessary attribute values.
        /// </summary>
        /// <returns></returns>
        public MessageStage UseGanymedeEndpointOptions()
        {
            if (PipelineDescriptor.Resource == null) return this;

            WithHttpVerb(PipelineDescriptor.Resource.GetVerb());

            WithHttpVersion(PipelineDescriptor.Resource.GetHttpVersion());

            WithTimeout(PipelineDescriptor.Resource.GetTimeout());

            WithCompletionOption(PipelineDescriptor.Resource.GetCompletionOption());

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
            PipelineDescriptor.RequestMessage.SetCancellationToken(cancellationToken);

            return this;
        }

        /// <summary>
        ///     Add <see cref="HttpCompletionOption" /> to the current <see cref="PipelineDescriptor" />.
        /// </summary>
        /// <param name="completionOption">The completion option</param>
        public MessageStage WithCompletionOption(HttpCompletionOption completionOption)
        {
            if (!Enum.IsDefined(typeof(HttpCompletionOption), completionOption))
                throw new InvalidEnumArgumentException(nameof(completionOption), (int) completionOption, typeof(HttpCompletionOption));

            PipelineDescriptor.RequestMessage.SetCompletionOption(completionOption);

            return this;
        }

        /// <summary>
        ///     Set the <see cref="HttpMethod" /> for the current <see cref="PipelineDescriptor" />.
        /// </summary>
        /// <param name="httpMethod">Method od the request</param>
        public MessageStage WithHttpVerb(HttpMethod httpMethod)
        {
            if (httpMethod == null) throw new ArgumentNullException(nameof(httpMethod));

            PipelineDescriptor.RequestMessage.Method = httpMethod;

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
            PipelineDescriptor.RequestMessage.Version = version ?? throw new ArgumentNullException(nameof(version));

            return this;
        }


        /// <summary>
        ///     Set a timeout for the request.
        /// </summary>
        /// <param name="timeout"></param>
        public MessageStage WithTimeout(TimeSpan timeout)
        {
            PipelineDescriptor.RequestMessage.SetTimeout(timeout);

            return this;
        }
    }
}