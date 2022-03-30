﻿using System.Collections.Generic;
using Application.StorageContracts;
using Domain.Entities;
using FluentValidation;


namespace Application.ValidationRules;


public static class PersonValidationsRules
{
    public static void PersonMustNotBeNull<T>(this IRuleBuilder<T, Person> ruleBuilder) =>
        ruleBuilder
            .NotNull().WithMessage($"{nameof(Cleaner.Person)} must be not null");


    public static void FirstNameRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage($"{nameof(Cleaner.Person.FirstName)} must not be null")
            .MaximumLength(PersonStorageContract.FirstNameMaxLength)
            .WithMessage(
                $"{nameof(Cleaner.Person.FirstName)} maximum length is {PersonStorageContract.FirstNameMaxLength}");


    public static void SurNameRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage($"{nameof(Cleaner.Person.SurName)} must not be null")
            .MaximumLength(PersonStorageContract.SurNameMaxLength)
            .WithMessage(
                $"{nameof(Cleaner.Person.SurName)} maximum length is {PersonStorageContract.SurNameMaxLength}");


    public static void PatronymicRule<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty().WithMessage($"{nameof(Cleaner.Person.Patronymic)} must not be null")
            .MaximumLength(PersonStorageContract.PatronymicMaxLength)
            .WithMessage(
                $"{nameof(Cleaner.Person.Patronymic)} maximum length is {PersonStorageContract.PatronymicMaxLength}");
}
