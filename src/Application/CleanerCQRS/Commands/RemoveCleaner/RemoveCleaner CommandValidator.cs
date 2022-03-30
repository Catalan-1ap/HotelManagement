using Application.ValidationRules;
using FluentValidation;


namespace Application.CleanerCQRS.Commands.RemoveCleaner;


public class RemoveCleanerCommandValidator : AbstractValidator<RemoveCleanerCommand>
{
    public RemoveCleanerCommandValidator()
    {
        RuleFor(c => c.Id)
            .IdMustBePositive();
    }
}
