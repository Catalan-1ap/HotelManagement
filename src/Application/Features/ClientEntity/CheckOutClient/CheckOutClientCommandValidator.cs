using Application.ValidationRules;
using FluentValidation;


namespace Application.Features.ClientEntity.CheckOutClient;


public sealed class CheckOutClientCommandValidator : AbstractValidator<CheckOutClientCommand>
{
    public CheckOutClientCommandValidator() => RuleFor(c => c.PayerPassport).PassportRule();
}
