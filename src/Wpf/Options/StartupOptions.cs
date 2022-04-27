namespace Wpf.Options;


public sealed class StartupOptions
{
    public const string SectionName = "Startup";

    public bool IsReCreateDatabaseRequired { get; set; }
}
