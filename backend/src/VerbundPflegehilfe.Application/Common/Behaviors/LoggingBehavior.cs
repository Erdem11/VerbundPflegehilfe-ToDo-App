namespace VerbundPflegehilfe.Application.Common.Behaviors;

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("VerbundPflege - Request: {Name} {@Request}", requestName, request);

        var timer = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        timer.Stop();

        if (timer.ElapsedMilliseconds > 500)
        {
            logger.LogWarning("VerbundPflege - Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", 
            requestName, timer.ElapsedMilliseconds, request);
        }

        return response;
    }
}