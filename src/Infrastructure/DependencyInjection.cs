using Application.Interfaces;
using Infrastructure.Localization;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;


public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, ServiceLifetime dbContextLifetime)
    {
        services.AddDbContext<ApplicationDbContext>(ApplicationDbContextOptionsFactory.Make, dbContextLifetime);

        services.Add(new(
            typeof(IApplicationDbContext),
            provider => provider.GetRequiredService<ApplicationDbContext>(),
            dbContextLifetime)
        );
        services.Add(new(
            typeof(IReadOnlyApplicationDbContext),
            typeof(ReadOnlyApplicationDbContext),
            dbContextLifetime)
        );

        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddSingleton<ILocalizer, Localizer>();
    }
}
