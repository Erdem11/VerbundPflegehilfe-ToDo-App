using System.Net;
using FluentValidation;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.API.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var result = Result<string>.Failure([
            "An internal error occurred."
        ]);

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                result = Result<string>.Failure(validationEx.Errors.Select(e => e.ErrorMessage));
                break;

            default:
                logger.LogError(exception, "Unhandled exception occurred.");
                break;
        }

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(result);
    }
}