using Application.StorageContracts;
using FluentValidation;


namespace Application.CleanerCQRS.Commands.CreateCleaner;


public class CreateCleanerCommandValidator : AbstractValidator<CreateCleanerCommand>
{
    public CreateCleanerCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("Имя должно быть указано")
            .MaximumLength(PersonStorageContract.FirstNameMaxLength);

        RuleFor(c => c.SurName)
            .NotEmpty().WithMessage("Фамилия должна быть указана")
            .MaximumLength(PersonStorageContract.SurNameMaxLength);

        RuleFor(c => c.Patronymic)
            .NotEmpty().WithMessage("Отчество должно быть указано")
            .MaximumLength(PersonStorageContract.PatronymicMaxLength);
    }
}
