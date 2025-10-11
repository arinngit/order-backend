using OrdersBackend.Domain.Models;

namespace OrdersBackend.Domain.Repositories;

public interface IUsersRepository
{
    Task<User> GetById(int id);
}