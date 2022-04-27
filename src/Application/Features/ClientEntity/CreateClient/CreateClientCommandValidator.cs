using Application.ValidationRules;
using FluentValidation;


namespace Application.Features.ClientEntity.CreateClient;


public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(c => c.FirstName)!.FirstNameRule();

        RuleFor(c => c.SurName)!.SurNameRule();

        RuleFor(c => c.Patronymic)!.PatronymicRule();

        RuleFor(c => c.Passport).PassportRule();
    }
}
