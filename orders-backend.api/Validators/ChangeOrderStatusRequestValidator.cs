using FluentValidation;
using OrdersBackend.Contracts.Requests;

namespace OrdersBackend.Api.Validators;

public class ChangeOrderStatusRequestValidator : AbstractValidator<ChangeOrderStatusRequest>
{
    public ChangeOrderStatusRequestValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().NotNull();
        RuleFor(x => x.StatusId).NotEmpty().NotNull();
    }
}