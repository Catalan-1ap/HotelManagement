using FluentValidation;


namespace Application.Common;


public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MatchesValidationRegex<T>(this IRuleBuilder<T, string> ruleBuilder, ValidationRegex regex) => ruleBuilder.Matches(regex.Regex).WithMessage(regex.Description);
}
