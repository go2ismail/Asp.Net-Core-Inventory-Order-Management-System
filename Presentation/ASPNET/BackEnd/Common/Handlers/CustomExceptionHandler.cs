using Application.Common.Exceptions;
using ASPNET.BackEnd.Common.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace ASPNET.BackEnd.Common.Handlers;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionHandler()
    {
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
        var statusCode = httpContext.Response.StatusCode != 200
            ? httpContext.Response.StatusCode
            : StatusCodes.Status500InternalServerError;

        var errorMessage = ex.Message;

        var result = new ApiErrorResult
        {
            Code = statusCode,
            Message = $"Exception: {errorMessage}",
            Error = new Error(ex.InnerException?.Message, ex.Source, ex.StackTrace, ex.GetType().Name)
        };

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

