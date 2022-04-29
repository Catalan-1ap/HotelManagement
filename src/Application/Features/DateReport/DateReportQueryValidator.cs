using Application.ValidationRules;
using FluentValidation;


namespace Application.Features.DateReport;


public sealed class DateReportQueryValidator : AbstractValidator<DateReportQuery>
{
    public DateReportQueryValidator() =>
        RuleFor(q => q.From)
            .MustBeLessThan(q => q.To);
}
