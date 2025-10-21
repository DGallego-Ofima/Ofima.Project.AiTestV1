using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ofima.Orders.Domain.Enums;

namespace Ofima.Orders.Domain.Entities;

[Table("Orders", Schema = "erp")]
public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Number { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.New;

    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }

    public DateTime? CanceledAt { get; set; }

    public int CreatedBy { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    // Navigation properties
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(CreatedBy))]
    public virtual User CreatedByUser { get; set; } = null!;

    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    // Business methods
    public bool CanBeEdited => Status == OrderStatus.New;

    public bool CanBeConfirmed => Status == OrderStatus.New && OrderLines.Any();

    public bool CanBeCanceled => Status == OrderStatus.Confirmed;

    public void CalculateTotals()
    {
        SubTotal = OrderLines.Sum(line => line.LineTotal);
        TaxAmount = SubTotal * 0.19m; // 19% IVA
        Total = SubTotal + TaxAmount;
    }

    public void Confirm(int userId)
    {
        if (!CanBeConfirmed)
            throw new InvalidOperationException($"Order {Number} cannot be confirmed in current state: {Status}");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Cancel(int userId)
    {
        if (!CanBeCanceled)
            throw new InvalidOperationException($"Order {Number} cannot be canceled in current state: {Status}");

        Status = OrderStatus.Canceled;
        CanceledAt = DateTime.UtcNow;
    }
}
