﻿using Application.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(ApplicationDbContextOptionsFactory.Make);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IReadOnlyApplicationDbContext, ReadOnlyApplicationDbContext>();

        services.AddTransient<IDateTimeService, DateTimeService>();

        return services;
    }
}
