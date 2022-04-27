using Application.StorageContracts;
using FluentValidation;


namespace Application.ValidationRules;


internal static class RoomValidationRules
{
    public static void RoomNumberRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage("Поле не должно быть пустым")
            .MaximumLength(RoomStorageContract.NumberMaxLength)
            .WithMessage($"Максимальная длина - {RoomStorageContract.NumberMaxLength}");
}
