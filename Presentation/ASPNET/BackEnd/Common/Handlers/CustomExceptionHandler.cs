using Application.Common.Exceptions;
using ASPNET.BackEnd.Common.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace ASPNET.BackEnd.Common.Handlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
        _exceptionHandlers = new()
            {
                { typeof(Exception), HandleException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            };
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (_exceptionHandlers.ContainsKey(exceptionType))
        {
            await _exceptionHandlers[exceptionType].Invoke(httpContext, exception);
            return true;
        }
        else
        {
            await HandleException(httpContext, exception);
            return true;
        }

    }

    private async Task HandleException(HttpContext httpContext, Exception ex)
    {
        var statusCode = httpContext.Response.StatusCode != StatusCodes.Status200OK
            ? httpContext.Response.StatusCode
            : StatusCodes.Status500InternalServerError;

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(ex, "Request rejected with {StatusCode} for {Method} {Path}",
                statusCode, httpContext.Request.Method, httpContext.Request.Path);
        }

        var result = new ApiErrorResult
        {
            Code = statusCode,
            Message = $"Exception: {ex.Message}",
            Error = new Error(null, null, null, ex.GetType().Name)
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(result);
    }

    private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        var result = new ApiErrorResult
        {
            Code = StatusCodes.Status403Forbidden,
            Message = ex.Message,
            Error = new Error(null, null, null, nameof(ForbiddenAccessException))
        };

        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync(result);
    }

}
