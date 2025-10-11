using OrdersBackend.Domain.Enums;

namespace OrdersBackend.Domain.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string OrdersNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public string Country { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Company { get; set; }
    public string DeliveryOption { get; set; }
    public string Address { get; set; }
    public string? Appartment { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Phone { get; set; }
    public decimal TaxAmout { get; set; }
    public decimal ShippingAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public int StatusId { get; set; }
    public string PromoCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Order()
    {

    }
}
