using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PM.API.Infrastructure.Configurations;
using PM.Domain.Models;

namespace PM.API.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly string _errorMessage;
        private readonly int _errorStatusCode;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IOptions<Messages> messages)
        {
            _next = next;
            _logger = logger;
            _errorMessage = messages.Value.DefaultError;
            _errorStatusCode = (int)HttpStatusCode.InternalServerError;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = _errorStatusCode;

            var errorResponse = new ErrorDTO
            {
                Code = _errorStatusCode,
                Message = _errorMessage,
                Exception = exception.GetType().Name
            };

            var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);

            _logger.LogError($"Response: {jsonErrorResponse}");

            await response.WriteAsync(jsonErrorResponse);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
