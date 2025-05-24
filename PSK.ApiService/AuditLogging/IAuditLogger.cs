namespace PSK.ApiService.AuditLogging;

public interface IAuditLogger
{
    Task LogAsync(HttpContext context, string controllerName, string actionName);
}