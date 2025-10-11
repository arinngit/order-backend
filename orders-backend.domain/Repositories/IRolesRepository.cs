using OrdersBackend.Domain.Models;

namespace OrdersBackend.Domain.Repositories;

public interface IRolesRepository
{
    Task<Role> GetById(int Id);
}
