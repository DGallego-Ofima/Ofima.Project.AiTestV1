using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ofima.Orders.Domain.Entities;

[Table("Products", Schema = "erp")]
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Stock? Stock { get; set; }
    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
