using FluentValidation;


namespace Application.ValidationRules;


public static class GenericRules
{
    public static void IdMustBePositive<T>(this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage("Id must be positive");
}
