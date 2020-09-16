using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solari.Sol;
using Solari.Vanth.Builders;

namespace Solari.Vanth
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly ApplicationOptions _appOptions;
        private readonly VanthOptions _options;


        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IOptions<VanthOptions> options,
        IOptions<ApplicationOptions> appOptions)
        {
            _next = next;
            _logger = logger;
            _appOptions = appOptions.Value;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"There was an error while executing the request: {ex}");
                if(_appOptions.IsInDevelopment())
                    await HandleExceptionAsync(httpContext, ex, true);
                await HandleExceptionAsync(httpContext, ex, _options.ReturnFullExceptionInProduction);

            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, bool shouldReturnStackTrace)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            IEnumerable<ErrorDetail> details = exception.ExtractDetailsFromException(shouldReturnStackTrace);
            IError error = new ErrorBuilder()
                          .WithCode(context.Response.StatusCode.ToString())
                          .WithTarget(context.Request.Path.ToString())
                          .WithDetail(details)
                          .WithMessage("An exception happened during the request")
                          .WithErrorType(CommonErrorType.Exception)
                          .Build();


            return context.Response.WriteAsync(new Result<string>().AddError(error).ToString());
        }
    }
}
