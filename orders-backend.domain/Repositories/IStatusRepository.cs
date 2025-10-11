using OrdersBackend.Domain.Models;

namespace OrdersBackend.Domain.Repositories;

public interface IStatusRepository
{
    Task<List<Status>> GetAll();
}