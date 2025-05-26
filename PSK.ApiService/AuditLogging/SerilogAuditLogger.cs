using System.Security.Claims;
using Serilog;

namespace PSK.ApiService.AuditLogging;

public class SerilogAuditLogger : IAuditLogger
{
    public Task LogAsync(HttpContext context, string controllerName, string actionName)
    {
        var userName = context.User.Identity?.Name;
        var role = context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        Log.Information(
            "ExecutionLogMiddleware: User {UserName} with role {Role} accessed {Controller}.{Action} at {Timestamp}",
            userName, role, controllerName, actionName, DateTime.UtcNow);

        return Task.CompletedTask;
    }
}