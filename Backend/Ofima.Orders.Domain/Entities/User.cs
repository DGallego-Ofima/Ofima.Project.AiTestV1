using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ofima.Orders.Domain.Entities;

[Table("Users", Schema = "sec")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    [Required]
    [MaxLength(40)]
    public string Role { get; set; } = "User";

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
