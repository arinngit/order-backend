using OrdersBackend.Domain.Models;
using OrdersBackend.Contracts.Requests;

namespace OrdersBackend.Business.Services.Abstractions;

public interface IOrdersService
{
    Task<Order> Add(AddOrderRequest order);
    Task<Boolean> Cancel(Guid orderId);
    Task<List<Order>> GetUsersOrders(int userId, int pageSize, int pageIndex);
    Task<Boolean> ChangeOrderStatus(int orderId, int statusId);
}
