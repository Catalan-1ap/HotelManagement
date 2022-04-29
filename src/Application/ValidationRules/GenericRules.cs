using System;
using System.Linq.Expressions;
using FluentValidation;


namespace Application.ValidationRules;


public static class GenericRules
{
    public static void IdMustBePositive<T>(this IRuleBuilder<T, int> ruleBuilder) =>
        ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage("Идентификатор должен быть позитивным");


    public static void MustBeLessThan<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder,
        Expression<Func<T, DateTime>> to
    ) =>
        ruleBuilder
            .LessThan(to)
            .WithMessage("Дата должна быть в прошлом");
}
