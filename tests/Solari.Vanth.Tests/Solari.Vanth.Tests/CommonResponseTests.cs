using System;
using System.Linq;
using Newtonsoft.Json;
using Solari.Vanth.Builders;
using Xunit;

namespace Solari.Vanth.Tests
{
    public class CommonResponseTests
    {
        private const string Result = "this is a test";
        private const string Error = "this is an error";


        [Fact]
        public void BuildCommonErrorResponse_Detail_Exception_ShouldSerializeToJsonSuccessfully()
        {
            ErrorDetail detail = new ErrorDetailBuilder()
                                                 .WithMessage(Error)
                                                 .WithException(new Exception("This is an Exception"))
                                                 .Build();
            Error error = new ErrorBuilder()
                                        .WithMessage(Error)
                                        .WithDetail(detail)
                                        .Build();
            string json = JsonConvert.SerializeObject(error);
            Assert.NotEqual(string.Empty, json);
        }

        [Fact]
        public void BuildCommonErrorResponse_Detail_ShouldContainDetail() { }

        [Fact]
        public void BuildCommonErrorResponse_Detail_ShouldContainExceptionAsDetail()
        {
            ErrorDetail detail = new ErrorDetailBuilder()
                                                 .WithMessage(Error)
                                                 .WithException(new Exception("This is an Exception"))
                                                 .Build();
            Error error = new ErrorBuilder()
                                        .WithMessage(Error)
                                        .WithDetail(detail)
                                        .Build();
            Assert.True(error.HasDetails);
            Assert.NotNull(error.Details.FirstOrDefault());
        }

        [Fact]
        public void BuildCommonErrorResponse_Detail_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ErrorDetailBuilder()
                                                       .WithException(new Exception("This is an Exception"))
                                                       .Build());
        }

        [Fact]
        public void BuildCommonResponse_Error_ShouldContainOnlyOneError()
        {
            Result<string> error = new ResultBuilder<string>().WithError(builder => builder.WithMessage(Error).Build()).Build();
            Assert.True(error.HasErrors);
            Assert.False(error.HasData);
            Assert.NotEmpty(error.Errors);
            Assert.Single(error.Errors);
        }

        [Fact]
        public void BuildCommonResponse_Error_ShouldSerializeToJsonSuccessfully()
        {
            Result<string> error = new ResultBuilder<string>().WithError(builder => builder.WithMessage(Error).Build()).Build();
            string json = JsonConvert.SerializeObject(error);
            Assert.NotEqual(string.Empty, json);
        }

        [Fact]
        public void BuildCommonResponse_Result_ShouldBeEqualsPrivateProperty()
        {
            Result<string> response = new ResultBuilder<string>().WithResult(Result).Build();
            Assert.Equal(Result, response.Data);
        }

        [Fact]
        public void BuildCommonResponse_Result_ShouldContainOnlyResult()
        {
            Result<string> response = new ResultBuilder<string>().WithResult(Result).Build();
            Assert.True(response.HasData);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public void BuildCommonResponse_Result_ShouldSerializeToJson_Successfully()
        {
            Result<string> response = new ResultBuilder<string>().WithResult(Result).Build();
            string json = JsonConvert.SerializeObject(response);
            Assert.NotEqual(string.Empty, json);
        }
    }
}