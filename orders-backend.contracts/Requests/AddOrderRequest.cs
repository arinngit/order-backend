using OrdersBackend.Domain.Enums;

namespace OrdersBackend.Contracts.Requests;

public class AddOrderRequest
{
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Country { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Company { get; set; }
    public int OrdersNumber { get; set; }
    public string Address { get; set; } = null!;
    public string DeliveryOption { get; set; } = null!;
    public string? Appartment { get; set; }
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public decimal TaxAmout { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public int StatusId { get; set; }
    public string PromoCode { get; set; } = null!;
    public List<OrderItemRequest> OrderItems { get; set; } = new();
}
