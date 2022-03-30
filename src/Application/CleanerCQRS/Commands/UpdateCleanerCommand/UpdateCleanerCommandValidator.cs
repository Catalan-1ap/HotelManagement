using Application.ValidationRules;
using FluentValidation;


namespace Application.CleanerCQRS.Commands.UpdateCleanerCommand;


public class UpdateCleanerCommandValidator : AbstractValidator<UpdateCleanerCommand>
{
    public UpdateCleanerCommandValidator()
    {
        RuleFor(c => c.Cleaner.Id)
            .IdMustBePositive();

        RuleFor(c => c.Cleaner.Person)!
            .PersonMustNotBeNull();

        RuleFor(c => c.Cleaner.Person!.FirstName)
            !.FirstNameRule();

        RuleFor(c => c.Cleaner.Person!.SurName)
            !.SurNameRule();

        RuleFor(c => c.Cleaner.Person!.Patronymic)
            !.PatronymicRule();
    }
}
