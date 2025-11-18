using ECommerceApp.EComm.Commons.Exceptions;
using ECommerceApp.EComm.Commons.Modals;
using System.Net;
using System.Text.Json;

namespace ECommerceApp.Middleware
{
    public class GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var traceId = context.TraceIdentifier;
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                TraceId = traceId,
                Timestamp = DateTime.UtcNow
            };

            switch (exception)
            {
                case AppException appEx:
                    errorResponse.StatusCode = appEx.StatusCode;
                    errorResponse.Message = appEx.Message;
                    errorResponse.ValidationErrors = appEx.ValidationErrors;
                    errorResponse.Details = _environment.IsDevelopment() ? appEx.StackTrace : null;
                    response.StatusCode = appEx.StatusCode;
                    _logger.LogWarning(appEx, "AppException occurred. TraceId: {TraceId}, StatusCode: {StatusCode}", 
                        traceId, appEx.StatusCode);
                    break;

                case ArgumentNullException argNullEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid request: Missing required parameter.";
                    errorResponse.Details = _environment.IsDevelopment() ? argNullEx.Message : null;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(argNullEx, "ArgumentNullException occurred. TraceId: {TraceId}", traceId);
                    break;

                case ArgumentException argEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid request: " + argEx.Message;
                    errorResponse.Details = _environment.IsDevelopment() ? argEx.StackTrace : null;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(argEx, "ArgumentException occurred. TraceId: {TraceId}", traceId);
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Message = "Unauthorized access.";
                    errorResponse.Details = _environment.IsDevelopment() ? unauthorizedEx.Message : null;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    _logger.LogWarning(unauthorizedEx, "UnauthorizedAccessException occurred. TraceId: {TraceId}", traceId);
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = "The requested resource was not found.";
                    errorResponse.Details = _environment.IsDevelopment() ? keyNotFoundEx.Message : null;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogWarning(keyNotFoundEx, "KeyNotFoundException occurred. TraceId: {TraceId}", traceId);
                    break;

                case InvalidOperationException invalidOpEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid operation: " + invalidOpEx.Message;
                    errorResponse.Details = _environment.IsDevelopment() ? invalidOpEx.StackTrace : null;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    _logger.LogWarning(invalidOpEx, "InvalidOperationException occurred. TraceId: {TraceId}", traceId);
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException dbEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "A database error occurred while processing your request.";
                    errorResponse.Details = _environment.IsDevelopment() ? dbEx.InnerException?.Message ?? dbEx.Message : null;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(dbEx, "Database exception occurred. TraceId: {TraceId}", traceId);
                    break;

                case TimeoutException timeoutEx:
                    errorResponse.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    errorResponse.Message = "The request timed out.";
                    errorResponse.Details = _environment.IsDevelopment() ? timeoutEx.Message : null;
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    _logger.LogWarning(timeoutEx, "TimeoutException occurred. TraceId: {TraceId}", traceId);
                    break;

                default:
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "An unexpected error occurred while processing your request.";
                    errorResponse.Details = _environment.IsDevelopment() ? exception.Message : null;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}, Message: {Message}", 
                        traceId, exception.Message);
                    break;
            }

            JsonSerializerOptions jsonOptions = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = _environment.IsDevelopment()
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, jsonOptions);
            await response.WriteAsync(jsonResponse);
        }
    }
}

