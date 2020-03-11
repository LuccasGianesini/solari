using System;
using System.Threading.Tasks;
using Solari.Ganymede.Domain;
using Solari.Ganymede.Extensions;
using Solari.Rhea;
using Solari.Vanth;

namespace Solari.Ganymede.Pipeline
{
    public class DeserializationStage<T>
    {
        private CommonResponse<GanymedeHttpResponse<T>> _commonResponse;
        private readonly GanymedeHttpResponse<T> _httpResponse;

        public DeserializationStage(CommonResponse<GanymedeHttpResponse<T>> commonResponse, GanymedeHttpResponse<T> httpResponse)
        {
            _commonResponse = commonResponse;
            _httpResponse = httpResponse;
        }


        public async Task<CommonResponse<GanymedeHttpResponse<T>>> AsString(bool stringifyRequestBody = false)
        {
            await _httpResponse.StringifyResponseBody();
            if (stringifyRequestBody)
            {
                await _httpResponse.StringifyRequestBody();
            }

            _commonResponse.AddResult(_httpResponse);
            return _commonResponse;
        }

        public async Task<CommonResponse<GanymedeHttpResponse<T>>> AsModel()
        {
            try
            {
                Maybe<T> deserialized = await _httpResponse
                                              .ResponseMessage
                                              .RequestMessage.GetContentDeserializer()
                                              .Deserialize<T>(_httpResponse.ResponseMessage.Content);
                if (deserialized.HasValue)
                {
                    _httpResponse.AddDeserializedContent(deserialized.Value);
                    _commonResponse.AddResult(_httpResponse);
                    return _commonResponse;
                }

                _commonResponse.AddError(builder => builder.WithMessage("The Maybe object is empty. " +
                                                                        "An error may occurred while deserializing the object." +
                                                                        "Does the response body contains any data?")
                                                           .Build());
                return _commonResponse;
            }
            catch (Exception e)
            {
                _commonResponse.AddError(builder => builder.WithDetail(detail => detail.WithException(e).Build())
                                                           .WithMessage(e.Message)
                                                           .WithErrorType(CommonErrorType.Exception)
                                                           .Build());
                return _commonResponse;
            }
        }

        public CommonResponse<GanymedeHttpResponse<T>> AsRaw()
        {
            _commonResponse.AddResult(_httpResponse);
            return _commonResponse;
        }
    }
}