using System;
using Application.ValidationRules;
using FluentValidation;
using Wpf.ViewModels;


namespace Wpf.Validators;


public sealed class DateReportViewModelValidator : AbstractValidator<DateReportViewModel>
{
    public DateReportViewModelValidator()
    {
        RuleFor(m => m.To)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Дата должна быть указана");

        RuleFor(m => m.From)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Дата должна быть указана");

        When(m => m.To is not null && m.From is not null,
            () =>
            {
                RuleFor(m => (DateTime)m.From)
                    .MustBeLessThan(m => (DateTime)m.To);
            });
    }
}
