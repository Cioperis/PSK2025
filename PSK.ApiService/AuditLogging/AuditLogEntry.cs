using MongoDB.Bson;

namespace PSK.ApiService.AuditLogging;

public class AuditLogEntry
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string UserName { get; set; }
    public string Role { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
}