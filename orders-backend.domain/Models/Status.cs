namespace OrdersBackend.Domain.Models;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}