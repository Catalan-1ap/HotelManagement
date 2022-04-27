using Application.ValidationRules;
using FluentValidation;


namespace Application.Features.CleanerEntity.RemoveCleaner;


public class RemoveCleanerCommandValidator : AbstractValidator<RemoveCleanerCommand>
{
    public RemoveCleanerCommandValidator() => RuleFor(c => c.Id).IdMustBePositive();
}
