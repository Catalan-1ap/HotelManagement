using Application.StorageContracts;
using Domain.Entities;
using FluentValidation;


namespace Application.ValidationRules;


internal static class RoomValidationRules
{
    public static void RoomNumberRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage($"{nameof(Room.Number)} must not be empty")
            .MaximumLength(RoomStorageContract.NumberMaxLength)
            .WithMessage($"{nameof(Room.Number)} maximum length is {RoomStorageContract.NumberMaxLength}");
}
