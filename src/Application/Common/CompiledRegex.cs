using System.Text.RegularExpressions;


namespace Application.Common;


public static class CompiledRegex
{
    public static readonly ValidationRegex OnlyLetters = new(
        new(@"^[А-Я]{1}[а-я]+$", RegexOptions.Compiled),
        "Разрешены только русские буквы любого регистра, без пробелов, цифр и спец-символов, первая буква должна быть заглавной"
    );

    public static readonly ValidationRegex Passport = new(
        new(@"^\d{2} \d{2} \d{6}$", RegexOptions.Compiled),
        "Серия и номер паспорта записываются в формате XX XX YYYYYY, где XX XX — 4-значная серия паспорта и YYYYYY — 6-значный номер паспорта."
    );
}
