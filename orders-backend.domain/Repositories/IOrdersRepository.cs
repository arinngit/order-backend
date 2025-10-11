using OrdersBackend.Domain.Models;

namespace OrdersBackend.Domain.Repositories;

public interface IOrdersRepository
{
    Task<Order> Add(Order order);
    Task<Boolean> Cancel(Guid orderId);
    Task<List<Order>> GetUsersOrders(int userId, int pageSize, int pageIndex);
    Task<Boolean> ChangeOrderStatus(int orderId, int statusId);
}
