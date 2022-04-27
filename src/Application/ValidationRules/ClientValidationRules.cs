using Application.Common;
using Application.StorageContracts;
using FluentValidation;


namespace Application.ValidationRules;


public static class ClientValidationRules
{
    public static void CityRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .MatchesValidationRegex(CompiledRegex.OnlyLetters)
            .MaximumLength(ClientStorageContract.CityMaxLength)
            .WithMessage(
                $"Максимальная длина - {ClientStorageContract.CityMaxLength}");


    public static void PassportRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .MatchesValidationRegex(CompiledRegex.Passport)
            .MaximumLength(ClientStorageContract.PassportMaxLength)
            .WithMessage($"Максимальная длина - {ClientStorageContract.PassportMaxLength}");
}
