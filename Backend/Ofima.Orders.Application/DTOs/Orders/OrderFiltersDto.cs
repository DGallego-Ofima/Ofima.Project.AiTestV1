using Ofima.Orders.Domain.Enums;

namespace Ofima.Orders.Application.DTOs.Orders;

public class OrderFiltersDto
{
    public OrderStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CustomerId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public void ValidateAndCorrect()
    {
        if (Page < 1) Page = 1;
        if (PageSize < 1) PageSize = 10;
        if (PageSize > 100) PageSize = 100;

        if (FromDate.HasValue && ToDate.HasValue && FromDate > ToDate)
        {
            (FromDate, ToDate) = (ToDate, FromDate);
        }
    }
}
