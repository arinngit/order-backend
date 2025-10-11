using FluentValidation;
using OrdersBackend.Contracts.Requests;

namespace OrdersBackend.Api.Validators;

public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
{
    public CancelOrderRequestValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order Id is required");
    }
}