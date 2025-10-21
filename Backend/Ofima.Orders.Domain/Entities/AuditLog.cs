using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ofima.Orders.Domain.Entities;

[Table("AuditLog", Schema = "erp")]
public class AuditLog
{
    [Key]
    public long Id { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(40)]
    public string Entity { get; set; } = string.Empty;

    public int EntityId { get; set; }

    [Required]
    [MaxLength(30)]
    public string Action { get; set; } = string.Empty;

    public DateTime At { get; set; } = DateTime.UtcNow;

    public string? Payload { get; set; }

    [MaxLength(45)]
    public string? IpAddress { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    // Factory methods
    public static AuditLog Create(int userId, string entity, int entityId, string action, object? payload = null, string? ipAddress = null, string? userAgent = null)
    {
        return new AuditLog
        {
            UserId = userId,
            Entity = entity,
            EntityId = entityId,
            Action = action,
            Payload = payload != null ? System.Text.Json.JsonSerializer.Serialize(payload) : null,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            At = DateTime.UtcNow
        };
    }
}
