using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace FinActions.Application.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }


    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception,
                        "Uma exceção ocorrendo no endpoint {method} {path}, status code: {statusCode}",
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        httpContext.Response.StatusCode);

        var problemDetail = new ProblemDetails
        {
            Type = exception?.GetType()?.Name,
            Status = httpContext.Response.StatusCode,
            Detail = exception?.InnerException?.Message,
            Title = exception?.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        };
        problemDetail.Extensions.Add("stackTrace", exception.StackTrace);

        return await _problemDetailsService.TryWriteAsync(new()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetail
        });
    }
}
