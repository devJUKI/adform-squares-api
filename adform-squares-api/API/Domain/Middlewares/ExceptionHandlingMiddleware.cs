using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Domain.Exceptions;

namespace API.Domain.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred");

        var exceptionBase = exception as ExceptionBase;

        var problemDetails = new ProblemDetails
        {
            Title = exceptionBase != null ? exceptionBase.MessageHeader : "An error occurred while processing your request.",
            Status = exceptionBase != null ? (int)exceptionBase.StatusCode : (int)HttpStatusCode.InternalServerError,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = problemDetails.Status.Value;

        return context.Response.WriteAsJsonAsync(problemDetails);

    }
}

