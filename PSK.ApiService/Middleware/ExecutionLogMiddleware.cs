using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using PSK.ApiService.AuditLogging;
using PSK.ApiService.Data;
using PSK.ServiceDefaults.Models;
using Serilog;
using ILogger = DnsClient.Internal.ILogger;

namespace PSK.ApiService.Middleware;

public class ExecutionLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly bool _enabled;
    private readonly string[] _excludedPaths;

    public ExecutionLogMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _enabled = configuration.GetValue<bool>("AuditLog:Enabled");
        _excludedPaths = configuration.GetSection("AuditLog:ExcludedPaths").Get<string[]>() ?? new string[0];
    }

    public async Task InvokeAsync(HttpContext context, IAuditLogger logger)
    {
        if (!_enabled || _excludedPaths.Any(p => context.Request.Path.StartsWithSegments(p)))
        {
            await _next(context);
            return;
        }

        var endpoint = context.GetEndpoint();

        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        var controllerName = controllerActionDescriptor?.ControllerName;
        var actionName = controllerActionDescriptor?.ActionName;

        await logger.LogAsync(context, controllerName, actionName);

        await _next(context);
    }
}