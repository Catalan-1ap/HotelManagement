using Application.ValidationRules;
using FluentValidation;


namespace Application.CQRS.ClientEntity.CreateClient;


public sealed class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(c => c.Person)!.PersonMustNotBeNull();

        RuleFor(c => c.Person.FirstName)!.FirstNameRule();

        RuleFor(c => c.Person.SurName)!.SurNameRule();

        RuleFor(c => c.Person.Patronymic)!.PatronymicRule();

        RuleFor(c => c.Passport).PassportRule();
    }
}
