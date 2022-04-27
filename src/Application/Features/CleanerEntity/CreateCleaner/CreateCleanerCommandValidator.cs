using Application.ValidationRules;
using FluentValidation;


namespace Application.Features.CleanerEntity.CreateCleaner;


public class CreateCleanerCommandValidator : AbstractValidator<CreateCleanerCommand>
{
    public CreateCleanerCommandValidator()
    {
        RuleFor(c => c.FirstName).FirstNameRule();

        RuleFor(c => c.SurName).SurNameRule();

        RuleFor(c => c.Patronymic).PatronymicRule();
    }
}
