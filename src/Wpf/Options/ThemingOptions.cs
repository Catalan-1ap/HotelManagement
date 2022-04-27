namespace Wpf.Options;


public sealed class ThemingOptions
{
    public const string SectionName = "Theming";

    public string Base { get; set; } = string.Empty;
    public string Primary { get; set; } = string.Empty;
    public string Secondary { get; set; } = string.Empty;
}
