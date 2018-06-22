using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PM.API.Infrastructure.Configurations;
using PM.Domain.Models;

namespace PM.API.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static string _errorMessage;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IOptions<Messages> messages)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _errorMessage = messages.Value.DefaultError;
                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            return WriteExceptionAsync(context, exception, statusCode);
        }

        private static Task WriteExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;
            return response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = new ErrorResponse
                {
                    Code = (int)statusCode,
                    Message = _errorMessage,
                    Exception = exception.GetType().Name
                }
            }));
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
