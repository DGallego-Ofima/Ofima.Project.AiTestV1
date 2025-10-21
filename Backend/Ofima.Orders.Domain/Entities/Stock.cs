using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ofima.Orders.Domain.Entities;

[Table("Stocks", Schema = "erp")]
public class Stock
{
    [Key]
    public int ProductId { get; set; }

    public int OnHand { get; set; } = 0;

    public int Reserved { get; set; } = 0;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int Available { get; private set; }

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    // Navigation properties
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; } = null!;

    // Business methods
    public bool CanReserve(int quantity)
    {
        return Available >= quantity;
    }

    public void ReserveStock(int quantity)
    {
        if (!CanReserve(quantity))
            throw new InvalidOperationException($"Insufficient stock. Available: {Available}, Requested: {quantity}");

        Reserved += quantity;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void ReleaseStock(int quantity)
    {
        if (Reserved < quantity)
            throw new InvalidOperationException($"Cannot release more stock than reserved. Reserved: {Reserved}, Requested: {quantity}");

        Reserved -= quantity;
        LastUpdatedAt = DateTime.UtcNow;
    }
}
