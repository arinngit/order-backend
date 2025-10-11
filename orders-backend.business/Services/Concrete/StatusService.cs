using OrdersBackend.Business.Services.Abstractions;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;

namespace OrdersBackend.Business.Services.Concrete;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _statusRepository;

    public StatusService(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    public async Task<List<Status>> GetAll()
    {
        return await _statusRepository.GetAll();
    }
}