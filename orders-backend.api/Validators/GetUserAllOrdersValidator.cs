using FluentValidation;
using OrdersBackend.Api.Validators;
using OrdersBackend.Contracts.Requests;

namespace OrdersBackend.Api.Validators;

public class GetUserAllOrdersValidator : AbstractValidator<GetUserAllOrders>
{
    public GetUserAllOrdersValidator()
    {
        RuleFor(x => x.PageIndex).NotEmpty().NotNull();
        RuleFor(x => x.PageSize).NotEmpty().NotNull();
    }
}