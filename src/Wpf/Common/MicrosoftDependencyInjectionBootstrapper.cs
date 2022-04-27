using System;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Stylet;


namespace Wpf.Common;


#nullable disable


public class MicrosoftDependencyInjectionBootstrapper<TRootViewModel> : BootstrapperBase where TRootViewModel : class
{
    private TRootViewModel _rootViewModel;
    private ServiceProvider _serviceProvider;
    protected virtual TRootViewModel RootViewModel => _rootViewModel ??= (TRootViewModel)GetInstance(typeof(TRootViewModel));

    public IServiceProvider ServiceProvider => _serviceProvider;


    protected override void ConfigureBootstrapper()
    {
        var services = new ServiceCollection();
        DefaultConfigureIoC(services);
        ConfigureIoC(services);
        _serviceProvider = services.BuildServiceProvider();
    }


    /// <summary>
    ///     Carries out default configuration of the IoC container. Override if you don't want to do this
    /// </summary>
    protected virtual void DefaultConfigureIoC(IServiceCollection services)
    {
        var viewManagerConfig = new ViewManagerConfig
        {
            ViewFactory = GetInstance,
            ViewAssemblies = new() { GetType().Assembly }
        };

        var viewManager = new ViewManager(viewManagerConfig);
        services.AddSingleton<IViewManager>(viewManager);
        RegisterViewModels(services, viewManager.ViewModelNameSuffix);
        RegisterViews(services, viewManager.ViewNameSuffix);
        RegisterValidation(services);

        services.AddTransient<MessageBoxView>();

        services.AddSingleton<IWindowManagerConfig>(this);
        services.AddSingleton<IWindowManager, WindowManager>();
        services.AddSingleton<IEventAggregator, EventAggregator>();
        services.AddTransient<IMessageBoxViewModel, MessageBoxViewModel>(); // Not singleton!
        // Also need a factory
        services.AddSingleton<Func<IMessageBoxViewModel>>(() => new MessageBoxViewModel());
    }


    private void RegisterViewModels(IServiceCollection services, string viewModelSuffix)
    {
        var viewModelsTypes = typeof(MicrosoftDependencyInjectionBootstrapper<>).Assembly
            .GetTypes()
            .Where(type => type.Name.EndsWith(viewModelSuffix));

        foreach (var viewModelType in viewModelsTypes)
            services.AddTransient(viewModelType);
    }


    private void RegisterViews(IServiceCollection services, string viewSuffix)
    {
        var viewsTypes = typeof(MicrosoftDependencyInjectionBootstrapper<>).Assembly
            .GetTypes()
            .Where(type => type.Name.EndsWith(viewSuffix));

        foreach (var viewType in viewsTypes)
            services.AddTransient(viewType);
    }


    private void RegisterValidation(IServiceCollection services)
    {
        services.AddTransient(typeof(IModelValidator<>), typeof(FluentModelValidator<>));

        services.AddValidatorsFromAssemblyContaining(typeof(MicrosoftDependencyInjectionBootstrapper<>));
    }


    /// <summary>
    ///     Override to add your own types to the IoC container.
    /// </summary>
    protected virtual void ConfigureIoC(IServiceCollection services) { }


    public override object GetInstance(Type type) => _serviceProvider.GetRequiredService(type);


    protected override void Launch() => base.DisplayRootView(RootViewModel);


    public override void Dispose()
    {
        base.Dispose();

        ScreenExtensions.TryDispose(_rootViewModel);
        _serviceProvider?.Dispose();
    }
}
#nullable restore
