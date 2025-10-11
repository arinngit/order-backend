using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using OrdersBackend.Api.AuthorizationRequirements;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;

namespace OrdersBackend.Api.AuthorizationHandlers;

public class AdminPolicyAuthorizationHandler : AuthorizationHandler<AdminPolicyAuthorizationRequirements>
{
    private readonly IRolesRepository _rolesRepository;

    public AdminPolicyAuthorizationHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminPolicyAuthorizationRequirements requirement)
    {
        foreach (var item in context.User.Claims)
        {
            Console.WriteLine($"{item.Type} - {item.Value}");
        }

        Int32.TryParse(context.User.Claims.FirstOrDefault(x => x.Type.ToString() == ClaimTypes.Role)!.Value, out int roleId);

        if (roleId == 0)
        {
            context.Fail(new AuthorizationFailureReason(this, "Wrong Role Id"));
            return;
        }

        Role role = await _rolesRepository.GetById(roleId);
        if (role.Name == "AppAdmin")
        {
            context.Succeed(requirement);
            Console.WriteLine("Admin !!!");
            return;
        }

        return;
    }
}
