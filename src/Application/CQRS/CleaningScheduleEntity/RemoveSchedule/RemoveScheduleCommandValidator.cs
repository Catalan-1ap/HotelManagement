using Application.ValidationRules;
using FluentValidation;


namespace Application.CQRS.CleaningScheduleEntity.RemoveSchedule;


public sealed class RemoveScheduleCommandValidator : AbstractValidator<RemoveScheduleCommand>
{
    public RemoveScheduleCommandValidator()
    {
        RuleFor(c => c.FloorNumber).IdMustBePositive();

        RuleFor(c => c.CleanerId).IdMustBePositive();
    }
}
