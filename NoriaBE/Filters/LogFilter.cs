using Microsoft.AspNetCore.Mvc.Filters;
using NoriaBE.Services;

namespace NoriaBE.Filters;

public class LogFilter : ActionFilterAttribute
{
    private readonly ILoggerService _logger;

    public LogFilter(ILoggerService logger)
    {
        _logger = logger;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        string body = string.Empty;
        if (request.ContentLength > 0 && request.Body.CanRead)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
        }

        var requestLog = $"[REQUEST] {request.Method} {request.Path}{request.QueryString}";
        if (!string.IsNullOrWhiteSpace(body))
            requestLog += $" | Body: {body}";

        _logger.Info(requestLog);

        var executedContext = await next();

        var statusCode = context.HttpContext.Response.StatusCode;
        _logger.Info($"[RESPONSE] {request.Method} {request.Path}{request.QueryString} => {statusCode}");
    }
}
