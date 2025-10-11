using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersBackend.Api.Routes;
using OrdersBackend.Contracts.Requests;
using OrdersBackend.Business.Services.Abstractions;
using OrdersBackend.Domain.Models;

namespace OrdersBackend.Api.Controllers;

[ApiController]
[Route(ApiRoutes.OrdersController.Base)]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpPatch(ApiRoutes.OrdersController.ChangeOrderStatus)]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<ActionResult> ChangeOrderStatus([FromBody] ChangeOrderStatusRequest request)
    {
        Boolean result = await _ordersService.ChangeOrderStatus(request.OrderId, request.StatusId);

        if (!result)
        {
            return Conflict(new
            {
                Message = "Something Went Wrong"
            });
        }

        return Ok();
    }

    [HttpGet(ApiRoutes.OrdersController.GetUsersOrders)]
    [Authorize(Policy = "AdminPolicy")]
    // NOTE - For Admin
    public async Task<ActionResult> GetAllUsers([FromRoute] int userId, [FromQuery] GetUserAllOrders request)
    {
        List<Order> orders = await _ordersService.GetUsersOrders(userId, request.PageSize, request.PageIndex);

        return Ok(orders);
    }

    [HttpGet(ApiRoutes.OrdersController.GetOrders)]
    [Authorize(Policy = "GetOrdersPolicy")]
    // NOTE - For User
    public async Task<ActionResult> GetOrders([FromRoute] int userId, [FromQuery] GetUserAllOrders request)
    {
        List<Order> orders = await _ordersService.GetUsersOrders(userId, request.PageSize, request.PageIndex);

        return Ok(orders);
    }

    [HttpPatch(ApiRoutes.OrdersController.Cancel)]
    [Authorize]
    public async Task<ActionResult> Cancel([FromBody] CancelOrderRequest request)
    {
        Boolean result = await _ordersService.Cancel(request.OrderId);

        if (!result)
        {
            return Conflict(new
            {
                Message = "Something Went Wrong"
            });
        }

        return Ok(new
        {
            Message = "Order Was Successfully Canceled"
        });
    }

    [HttpPost(ApiRoutes.OrdersController.Add)]
    [Authorize]
    public async Task<ActionResult> Add([FromBody] AddOrderRequest request)
    {
        Order order = await _ordersService.Add(request);

        if (order.UserId == 0)
        {
            return BadRequest(new { Message = "Could not add order" });
        }

        return Ok(order);
    }
}
