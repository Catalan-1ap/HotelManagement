﻿using Application.ValidationRules;
using FluentValidation;


namespace Application.Features.CleaningScheduleEntity.CreateSchedule;


public sealed class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(c => c.FloorNumber).IdMustBePositive();

        RuleFor(c => c.CleanerId).IdMustBePositive();
    }
}
