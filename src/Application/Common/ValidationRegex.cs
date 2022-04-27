using System.Text.RegularExpressions;


namespace Application.Common;


public record ValidationRegex(Regex Regex, string Description);
