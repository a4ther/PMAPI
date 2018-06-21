using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PM.Domain.Models;

namespace PM.API.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (exception is HttpRequestException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

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
                    Message = exception.Message,
                    Exception = exception.GetType().Name
                }
            }));
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
