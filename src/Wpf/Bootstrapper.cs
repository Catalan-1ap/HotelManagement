using Application;
using Develop;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wpf.Common;
using Wpf.Options;
using Wpf.ViewModels;


namespace Wpf;


public sealed class Bootstrapper : MicrosoftDependencyInjectionBootstrapper<ShellViewModel>
{
    private readonly IConfiguration _configuration;


    public Bootstrapper() =>
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


    protected override void Launch()
    {
        var startupSettings = ServiceProvider.GetRequiredService<IOptions<StartupOptions>>().Value;

        if (startupSettings.IsReCreateDatabaseRequired)
        {
            ApplicationDbInitializer.ReCreate();
            SeedData.Seed();
        }

        base.Launch();
    }


    protected override void ConfigureIoC(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();

        services.Configure<StartupOptions>(_configuration.GetSection(nameof(StartupOptions)));
    }
}
