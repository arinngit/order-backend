namespace OrdersBackend.Domain.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? ProductId { get; set; }
    public int? VariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string? SizeName { get; set; }
    public string? ColorName { get; set; }
    public string? ColorHex { get; set; }
}