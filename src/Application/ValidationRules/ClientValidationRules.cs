using Application.StorageContracts;
using Domain.Entities;
using FluentValidation;


namespace Application.ValidationRules;


public static class ClientValidationRules
{
    public static void CityRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage($"{nameof(Client.City)} must not be empty")
            .MaximumLength(ClientStorageContract.CityMaxLength)
            .WithMessage(
                $"{nameof(Client.City)} maximum length is {ClientStorageContract.CityMaxLength}");


    public static void PassportRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage($"{nameof(Client.Passport)} must not be empty")
            .MaximumLength(ClientStorageContract.PassportMaxLength)
            .WithMessage($"{nameof(Client.Passport)} maximum length is {ClientStorageContract.PassportMaxLength}");
}
