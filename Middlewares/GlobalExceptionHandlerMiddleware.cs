
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;
using System.Text.Json;

namespace bca.api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            using var scope = _scopeFactory.CreateScope();
            //var exceptionService = scope.ServiceProvider.GetRequiredService<IExceptionLogService>();

            var statusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
                ArgumentException => (int)HttpStatusCode.BadRequest, // 400
                _ => (int)HttpStatusCode.InternalServerError // 500
            };

            string requestBody = string.Empty;

            try
            {
                if (context.Request.HasFormContentType)
                {
                    var form = await context.Request.ReadFormAsync();

                    var nonFileFields = form
                        .Where(f => !form.Files.Any(file => file.Name == f.Key))
                        .ToDictionary(f => f.Key, f => f.Value.ToString());

                    requestBody = JsonSerializer.Serialize(nonFileFields);
                }
                else if (context.Request.ContentLength > 0 &&
                         (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put))
                {
                    context.Request.Body.Position = 0;
                    using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }
            catch (Exception ex)
            {
                requestBody = $"[Failed to read request body: {ex.Message}]";
            }

            //await exceptionService.LogExceptionAsync(new ExceptionLog
            //{
            //    ExceptionType = exception.GetType().Name,
            //    Message = exception.Message,
            //    StackTrace = exception.StackTrace ?? "NA",
            //    InnerException = exception.InnerException?.ToString() ?? "NA",
            //    Source = exception.Source ?? "NA",
            //    Path = context.Request.Path,
            //    QueryString = context.Request.QueryString.ToString(),
            //    Method = context.Request.Method,
            //    RequestBody = requestBody,
            //    StatusCode = statusCode,
            //    CreatedAt = DateTime.UtcNow
            //});

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = context.Response.StatusCode,
                message = exception.Message,
                detail = exception.InnerException?.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
