using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersBackend.Api.Routes;
using OrdersBackend.Business.Services.Abstractions;
using OrdersBackend.Domain.Models;

namespace OrdersBackend.Api.Controllers;

[ApiController]
[Route(ApiRoutes.StatusController.Base)]
[Authorize(Policy = "AdminPolicy")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet(ApiRoutes.StatusController.GetAll)]
    public async Task<ActionResult<List<Status>>> GetAll()
    {
        return await _statusService.GetAll();
    }
}