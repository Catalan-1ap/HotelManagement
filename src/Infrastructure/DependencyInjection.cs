using Application.Interfaces;
using Infrastructure.Localization;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;


public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(ApplicationDbContextOptionsFactory.Make);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IReadOnlyApplicationDbContext, ReadOnlyApplicationDbContext>();

        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddSingleton<ILocalizer, Localizer>();
    }
}
