using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Web.Middlewares
{
    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;

        public ApiExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IHostEnvironment env)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _env = env;
            _logger = loggerFactory?.CreateLogger<ApiExceptionHandlerMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = context.Request.ContentType;
                if (_env.IsDevelopment())
                {
                    var json = JsonConvert.SerializeObject(ex);
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    var json = JsonConvert.SerializeObject(ex.Message);
                    await context.Response.WriteAsync(json);
                }

                _logger.LogError(ex, "Api error");
                return;
            }
        }
    }
}