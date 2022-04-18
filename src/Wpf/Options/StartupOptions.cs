namespace Wpf.Options;


public class StartupOptions
{
    public const string SectionName = "Startup";

    public bool IsReCreateDatabaseRequired { get; set; }
}
