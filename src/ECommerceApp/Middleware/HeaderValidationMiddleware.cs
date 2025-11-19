using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace ECommerceApp.Middleware
{
    public class HeaderValidationMiddleware(RequestDelegate next, ILogger<HeaderValidationMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<HeaderValidationMiddleware> _logger = logger;

        private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "/api/register/create",
            "/api/registration/create",
            "/api/Auth/token"
        };

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var isControllerAction = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>() != null;

            if (!isControllerAction || ShouldSkipValidation(context.Request.Path))
            {
                await _next(context);
                return;
            }

            if (!TryGetHeaderValue(context.Request.Headers, "CorrelationId", out var correlationId))
            {
                await WriteMissingHeaderResponse(context, "CorrelationId");
                return;
            }

            if (!TryGetHeaderValue(context.Request.Headers, "AuthToken", out var authToken))
            {
                await WriteMissingHeaderResponse(context, "AuthToken");
                return;
            }

            context.Items["CorrelationId"] = correlationId.ToString();
            context.Items["AuthToken"] = authToken.ToString();

            await _next(context);
        }

        private static bool ShouldSkipValidation(PathString path)
        {
            if (!path.HasValue)
            {
                return true;
            }

            return ExcludedPaths.Contains(path.Value);
        }

        private static bool TryGetHeaderValue(IHeaderDictionary headers, string headerName, out StringValues value)
        {
            if (headers.TryGetValue(headerName, out value) && !StringValues.IsNullOrEmpty(value))
            {
                return true;
            }

            if (headerName.Equals("CorrelationId", StringComparison.OrdinalIgnoreCase)
                && headers.TryGetValue("X-Correlation-Id", out value) && !StringValues.IsNullOrEmpty(value))
            {
                return true;
            }

            value = StringValues.Empty;
            return false;
        }

        private async Task WriteMissingHeaderResponse(HttpContext context, string headerName)
        {
            _logger.LogWarning("Request blocked due to missing header {HeaderName}. Path: {Path}", headerName, context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var payload = new
            {
                traceId = context.TraceIdentifier,
                statusCode = StatusCodes.Status400BadRequest,
                message = $"Missing required header: {headerName}"
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload, options));
        }
    }
}


