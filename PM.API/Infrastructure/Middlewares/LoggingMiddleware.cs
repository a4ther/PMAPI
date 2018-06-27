using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PM.API.Models.Logging;

namespace PM.API.Infrastructure.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var requestStream = new MemoryStream())
            using (var responseStream = new MemoryStream())
            {
                var originalResponseStream = httpContext.Response.Body;
                httpContext.Response.Body = responseStream;

                try
                {
                    var request = await FormatRequest(httpContext.Request, requestStream);
                    _logger.LogDebug($"Request: {request}");

                    await _next.Invoke(httpContext);

                    var response = await FormatResponse(httpContext.Response, responseStream);
                    _logger.LogDebug($"Response: {response}");

                    await responseStream.CopyToAsync(originalResponseStream);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    httpContext.Response.Body = originalResponseStream;
                }
            }
        }

        private static async Task<string> FormatRequest(HttpRequest httpRequest, MemoryStream requestStream)
        {
            var protocol = httpRequest.IsHttps ? "https" : "http";
            var route = $"{httpRequest.Method} {protocol}://{httpRequest.Host.Value}{httpRequest.Path}";

            using (var bodyReader = new StreamReader(httpRequest.Body))
            {
                var bodyAsText = await bodyReader.ReadToEndAsync();
                var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                await requestStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                requestStream.Seek(0, SeekOrigin.Begin);
                httpRequest.Body = requestStream;


                var requestLog = new RequestLog
                {
                    Body = JsonConvert.DeserializeObject(bodyAsText),
                    Route = route,
                    QueryString = httpRequest.QueryString.Value
                };

                return Regex.Unescape(JsonConvert.SerializeObject(requestLog, Formatting.Indented));
            }
        }

        private static async Task<string> FormatResponse(HttpResponse httpResponse, MemoryStream responseStream)
        {
            var buffer = new byte[responseStream.Length];
            responseStream.Seek(0, SeekOrigin.Begin);
            await responseStream.ReadAsync(buffer, 0, buffer.Length);
            responseStream.Seek(0, SeekOrigin.Begin);
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            var responseLog = new ResponseLog
            {
                Body = JsonConvert.DeserializeObject(bodyAsText),
                StatusCode = httpResponse.StatusCode
            };

            return Regex.Unescape(JsonConvert.SerializeObject(responseLog, Formatting.Indented));
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
