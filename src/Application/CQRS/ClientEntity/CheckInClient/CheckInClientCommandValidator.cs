using Application.ValidationRules;
using FluentValidation;


namespace Application.CQRS.ClientEntity.CheckInClient;


public sealed class CheckInClientCommandValidator : AbstractValidator<CheckInClientCommand>
{
    public CheckInClientCommandValidator()
    {
        RuleFor(c => c.Passport).PassportRule();

        RuleFor(c => c.City).CityRule();

        RuleFor(c => c.RoomNumber).RoomNumberRule();
    }
}
