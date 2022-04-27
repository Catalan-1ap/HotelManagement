using Application.Common;
using Application.StorageContracts;
using FluentValidation;


namespace Application.ValidationRules;


public static class PersonValidationRules
{
    public static void FirstNameRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .MatchesValidationRegex(CompiledRegex.OnlyLetters)
            .MaximumLength(PersonStorageContract.FirstNameMaxLength)
            .WithMessage(
                $"Максимальная длина - {PersonStorageContract.FirstNameMaxLength}");


    public static void SurNameRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .MatchesValidationRegex(CompiledRegex.OnlyLetters)
            .MaximumLength(PersonStorageContract.SurNameMaxLength)
            .WithMessage(
                $"Максимальная длина - {PersonStorageContract.SurNameMaxLength}");


    public static void PatronymicRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .MatchesValidationRegex(CompiledRegex.OnlyLetters)
            .MaximumLength(PersonStorageContract.PatronymicMaxLength)
            .WithMessage(
                $"Максимальная длина - {PersonStorageContract.PatronymicMaxLength}");
}
