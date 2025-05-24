using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using PSK.ApiService.Data;
using PSK.ServiceDefaults.Models;
using Serilog;

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

    public async Task InvokeAsync(HttpContext context)
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

        var userName = context.User.Identity?.Name;
        var role = context.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value).FirstOrDefault();

        Log.Information(
            "ExecutionLogMiddleware: User {UserName} with role {Role} accessed {Controller}.{Action} at {Timestamp}",
            userName, role, controllerName, actionName, DateTime.UtcNow);

        await _next(context);
        return;
    }
}