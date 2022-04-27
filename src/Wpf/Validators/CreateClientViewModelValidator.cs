using Application.ValidationRules;
using FluentValidation;
using Wpf.ViewModels;


namespace Wpf.Validators;


public sealed class CreateClientViewModelValidator : AbstractValidator<CreateClientViewModel>
{
    public CreateClientViewModelValidator()
    {
        RuleFor(m => m.FirstName).FirstNameRule();

        RuleFor(m => m.SurName).SurNameRule();

        RuleFor(m => m.Patronymic).PatronymicRule();

        RuleFor(m => m.Passport).PassportRule();
    }
}


public sealed class CheckInClientViewModelValidator : AbstractValidator<CheckInClientViewModel>
{
    public CheckInClientViewModelValidator() => RuleFor(c => c.City).CityRule();
}
