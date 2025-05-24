using System.Security.Claims;
using MongoDB.Driver;

namespace PSK.ApiService.AuditLogging;

public class MongoDbAuditLogger : IAuditLogger
{
    private readonly IMongoCollection<AuditLogEntry> _collection;

    public MongoDbAuditLogger(IMongoDatabase database)
    {
        _collection = database.GetCollection<AuditLogEntry>("auditLogs");
    }

    public async Task LogAsync(HttpContext context, string controllerName, string actionName)
    {
        var userName = context.User.Identity?.Name;
        var role = context.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        var entry = new AuditLogEntry
        {
            UserName = userName,
            Role = role,
            Controller = controllerName,
            Action = actionName,
            Timestamp = DateTime.UtcNow
        };

        await _collection.InsertOneAsync(entry);
    }
}