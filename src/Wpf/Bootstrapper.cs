﻿using System;
using System.Windows;
using System.Windows.Threading;
using Application;
using Application.Exceptions;
using Develop;
using Infrastructure;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stylet;
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


    public static IServiceProvider GlobalServiceProvider { get; private set; } = null!;


    protected override void Launch()
    {
        GlobalServiceProvider = ServiceProvider;

        StartUp();
        Theming();

        base.Launch();
    }


    private void StartUp()
    {
        var startupOptions = ServiceProvider.GetRequiredService<IOptions<StartupOptions>>().Value;

        if (startupOptions.IsReCreateDatabaseRequired || ApplicationDbInitializer.IsNotExists())
        {
            ApplicationDbInitializer.ReCreate();
            SeedData.Seed();
        }
    }


    private void Theming()
    {
        var themingOptions = ServiceProvider.GetRequiredService<IOptions<ThemingOptions>>().Value;
        var paletteHelper = new PaletteHelper();
        var theme = paletteHelper.GetTheme();

        theme.SetBaseTheme(themingOptions.Base switch
        {
            "Dark" => Theme.Dark,
            _ => Theme.Light
        });

        if (Enum.TryParse(themingOptions.Primary, out MaterialDesignColor primary))
            theme.SetPrimaryColor(SwatchHelper.Lookup[primary]);

        if (Enum.TryParse(themingOptions.Secondary, out MaterialDesignColor secondary))
            theme.SetPrimaryColor(SwatchHelper.Lookup[secondary]);

        theme.AdjustColors();

        paletteHelper.SetTheme(theme);
    }


    protected override void ConfigureIoC(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();

        services.Configure<StartupOptions>(_configuration.GetSection(StartupOptions.SectionName));
        services.Configure<ThemingOptions>(_configuration.GetSection(ThemingOptions.SectionName));
    }


    protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
    {
        switch (e.Exception)
        {
            case BusinessException ex:
                HandleBusinessException(ex);
                e.Handled = true;
                break;
            default:
                Environment.FailFast(e.Exception.Message);
                break;
        }
    }


    private void HandleBusinessException(BusinessException exception) =>
        ServiceProvider
            .GetRequiredService<IWindowManager>()
            .ShowMessageBox(
                messageBoxText: exception.Message,
                caption: "Произошла ошибка",
                buttons: MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                textAlignment: TextAlignment.Center);
}
