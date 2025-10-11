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

    public async Task<Order> Add(AddOrderRequest order)
    {
        Order newOrder = new Order
        {
            UserId = order.UserId,
            OrdersNumber = order.OrdersNumber.ToString(),
            TotalAmount = order.TotalAmount,
            Country = order.Country,
            FirstName = order.FirstName,
            LastName = order.LastName,
            Company = order.Company,
            DeliveryOption = order.DeliveryOption,
            Address = order.Address,
            Appartment = order.Appartment,
            PostalCode = order.PostalCode,
            City = order.City,
            Phone = order.Phone,
            TaxAmout = order.TaxAmout,
            ShippingAmount = order.ShippingAmount,
            DiscountAmount = order.DiscountAmount,
            FinalAmount = order.FinalAmount,
            StatusId = order.StatusId,
            PromoCode = order.PromoCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _orders.Add(newOrder);
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
            return [];
        }

        return await _orders.GetUsersOrders(userId, pageSize, pageIndex);
    }
}
