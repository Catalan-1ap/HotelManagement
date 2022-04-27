using Application.ValidationRules;
using FluentValidation;
using Wpf.ViewModels;


namespace Wpf.Validators;


public sealed class CreateCleanerViewModelValidator : AbstractValidator<CreateCleanerViewModel>
{
    public CreateCleanerViewModelValidator()
    {
        RuleFor(m => m.FirstName).FirstNameRule();

        RuleFor(m => m.SurName).SurNameRule();

        RuleFor(m => m.Patronymic).PatronymicRule();
    }
}
