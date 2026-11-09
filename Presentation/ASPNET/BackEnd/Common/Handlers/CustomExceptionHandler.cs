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

}

