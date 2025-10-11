using OrdersBackend.Domain.Models;

namespace OrdersBackend.Business.Services.Abstractions;

public interface IStatusService
{
    Task<List<Status>> GetAll();
}