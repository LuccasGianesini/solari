using System.Text;
using Solari.Ganymede.ContentSerializers;
using Solari.Ganymede.Domain.Options;
using Solari.Ganymede.Extensions;
using Solari.Ganymede.Framework;

namespace Solari.Ganymede.Pipeline
{
    public class ContentStage : IPipelineStage
    {
        public ContentStage(PipelineContext pipelineContext) { PipelineContext = pipelineContext; }

        public PipelineContext PipelineContext { get; }

        public static implicit operator PipelineContext(ContentStage contentStage) { return contentStage.PipelineContext; }

        /// <summary>
        ///     Use the <see cref="GanymedeRequestResource" /> object to set the necessary attribute values.
        /// </summary>
        /// <returns></returns>
        public ContentStage UseGanymedeEndpointOptions()
        {
            if (PipelineContext.Resource == null) return this;

            WithContentSerializer(PipelineContext.Resource.GetSerializer());
            WithContentDeserializer(PipelineContext.Resource.GetDeserializer());

            return this;
        }

        /// <summary>
        ///     Serializes the content into the correct output.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public ContentStage SerializeContent(object content, string contentType = "", Encoding encoding = null)
        {
            PipelineContext.RequestMessage.Content = SerializerManager.SendToSerializer(content, PipelineContext.RequestMessage,
                                                                                           contentType, encoding);

            return this;
        }


        /// <summary>
        ///     Set the content deserializer (<see cref="IContentSerializer" />) for the current
        ///     <see cref="System.Net.Http.HttpRequestMessage" />.
        ///     If null, <see cref="JsonContentSerializer" /> will be used.
        /// </summary>
        /// <remarks>
        ///     Available Serializers:
        ///     JSON
        ///     URLENCODED
        ///     XML
        /// </remarks>
        /// <param name="contentSerializer">Serializer class instance</param>
        public ContentStage WithContentDeserializer(IContentDeserializer contentSerializer)
        {
            PipelineContext.RequestMessage.SetContentDeserializer(contentSerializer);

            return this;
        }


        /// <summary>
        ///     Set the content serializer (<see cref="IContentSerializer" />) for the current
        ///     <see cref="System.Net.Http.HttpRequestMessage" />.
        ///     If null, <see cref="JsonContentSerializer" /> will be used.
        /// </summary>
        /// <remarks>
        ///     Available Serializers:
        ///     JSON
        ///     URLENCODED
        ///     XML
        /// </remarks>
        /// <param name="contentSerializer">Serializer class instance</param>
        public ContentStage WithContentSerializer(IContentSerializer contentSerializer)
        {
            PipelineContext.RequestMessage.SetContentSerializer(contentSerializer);

            return this;
        }
    }
}