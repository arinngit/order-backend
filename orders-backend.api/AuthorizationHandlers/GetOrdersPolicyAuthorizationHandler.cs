using Microsoft.AspNetCore.Authorization;
using OrdersBackend.Api.AuthorizationRequirements;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;

namespace OrdersBackend.Api.AuthorizationHandlers;

public class GetOrdersPolicyAuthorzationHandler : AuthorizationHandler<GetOrdersPolicyAuthorizationRequirements>
{
    private readonly IUsersRepository _users;

    public GetOrdersPolicyAuthorzationHandler(IUsersRepository users)
    {
        _users = users;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GetOrdersPolicyAuthorizationRequirements requirement)
    {
        Console.WriteLine("Get Orders handler");

        foreach (var item in context.User.Claims)
        {
            Console.WriteLine($"{item.Type} - {item.Value}");
        }

        Int32.TryParse(context.User.Claims.FirstOrDefault(x => x.Type.ToString() == "id")!.Value, out int id);

        User user = await _users.GetById(id);

        if (user.Id == 0)
        {
            context.Fail(new AuthorizationFailureReason(this, "User Not Found"));
            return;
        }

        context.Succeed(requirement);
    }
}