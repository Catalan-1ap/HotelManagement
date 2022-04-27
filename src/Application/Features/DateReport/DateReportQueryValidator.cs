using FluentValidation;


namespace Application.Features.DateReport;


public sealed class DateReportQueryValidator : AbstractValidator<DateReportQuery>
{
    public DateReportQueryValidator() =>
        RuleFor(q => q.From)
            .LessThan(q => q.To)
            .WithMessage($"{nameof(DateReportQuery.From)} must be greater than {nameof(DateReportQuery.To)}");
}
