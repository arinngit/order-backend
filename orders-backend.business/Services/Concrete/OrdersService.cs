using OrdersBackend.Business.Services.Abstractions;
using OrdersBackend.Contracts.Requests;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;

namespace OrdersBackend.Business.Services.Concrete;

public class OrdersService : IOrdersService
{
    private readonly IOrdersRepository _orders;
    private readonly IUsersRepository _users;

    public OrdersService(IOrdersRepository orders, IUsersRepository users)
    {
        _orders = orders;
        _users = users;
    }

    public async Task<Order> Add(AddOrderRequest orderRequest)
    {
        if (orderRequest.OrderItems != null)
        {
            for (int i = 0; i < orderRequest.OrderItems.Count; i++)
            {
                var item = orderRequest.OrderItems[i];
                Console.WriteLine($"  Item[{i}]: ProductId='{item.ProductId}', VariantId={item.VariantId}, Quantity={item.Quantity}, UnitPrice={item.UnitPrice}, TotalPrice={item.TotalPrice}, Size='{item.SizeName}', Color='{item.ColorName}'");
            }
        }
    
        Order newOrder = new Order
        {
            UserId = orderRequest.UserId,
            OrdersNumber = orderRequest.OrdersNumber.ToString(),
            TotalAmount = orderRequest.TotalAmount,
            Country = orderRequest.Country,
            FirstName = orderRequest.FirstName,
            LastName = orderRequest.LastName,
            Company = orderRequest.Company,
            DeliveryOption = orderRequest.DeliveryOption,
            Address = orderRequest.Address,
            Appartment = orderRequest.Appartment,
            PostalCode = orderRequest.PostalCode,
            City = orderRequest.City,
            Phone = orderRequest.Phone,
            TaxAmout = orderRequest.TaxAmout,
            ShippingAmount = orderRequest.ShippingAmount,
            DiscountAmount = orderRequest.DiscountAmount,
            FinalAmount = orderRequest.FinalAmount,
            StatusId = orderRequest.StatusId,
            PromoCode = orderRequest.PromoCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    
        Console.WriteLine($"Creating order in DB...");
    
        var createdOrder = await _orders.Add(newOrder);
    
        Console.WriteLine($"Order created with Id: {createdOrder.Id}");
    
        if (createdOrder.Id == 0)
        {
            Console.WriteLine("ERROR: Order creation failed (Id = 0)");
            return new Order();
        }
    
        var orderItems = orderRequest.OrderItems.Select(item => new OrderItem
        {
            OrderId = createdOrder.Id,
            ProductId = item.ProductId,
            VariantId = item.VariantId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.TotalPrice,
            SizeName = item.SizeName,
            ColorName = item.ColorName,
            ColorHex = item.ColorHex
        }).ToList();
    
        Console.WriteLine($"Prepared {orderItems.Count} OrderItems for insertion:");
    
        foreach (var item in orderItems)
        {
            Console.WriteLine($"  -> OrderId={item.OrderId}, ProductId='{item.ProductId}', VariantId={item.VariantId}, Quantity={item.Quantity}");
        }
    
        if (orderItems.Any())
        {
            Console.WriteLine("Inserting OrderItems into DB...");
            bool itemsAdded = await _orders.AddOrderItems(orderItems);
    
            if (!itemsAdded)
            {
                Console.WriteLine($"ERROR: Failed to add OrderItems for OrderId: {createdOrder.Id}");
            }
            else
            {
                Console.WriteLine($"SUCCESS: {orderItems.Count} OrderItems inserted for OrderId: {createdOrder.Id}");
            }
        }
        else
        {
            Console.WriteLine("No OrderItems to insert.");
        }
    
        Console.WriteLine("=== OrdersService.Add END ===");
        return createdOrder;
    }

    public async Task<Boolean> Cancel(Guid orderId)
    {
        return await _orders.Cancel(orderId);
    }

    public async Task<bool> ChangeOrderStatus(int orderId, int statusId)
    {
        return await _orders.ChangeOrderStatus(orderId, statusId);
    }

    public async Task<List<Order>> GetUsersOrders(int userId, int pageSize, int pageIndex)
    {
        User user = await _users.GetById(userId);

        if (user.Id == 0)
        {
            Console.WriteLine("User not found");
            return [];
        }

        return await _orders.GetUsersOrders(userId, pageSize, pageIndex);
    }
}
