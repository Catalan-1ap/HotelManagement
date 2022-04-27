using Microsoft.Extensions.DependencyInjection;
using Stylet;


namespace Wpf.Extensions;


public static class ScreenExtensions
{
    public static (bool dialogResult, TViewModel viewModel) ShowDialog<TViewModel>(this Screen screen)
        where TViewModel : Screen
    {
        var viewModel = Bootstrapper.GlobalServiceProvider.GetRequiredService<TViewModel>();

        return (ShowDialog(screen, viewModel), viewModel);
    }


    public static bool ShowDialog<TViewModel>(this Screen screen, TViewModel viewModel)
        where TViewModel : Screen
    {
        var serviceProvider = Bootstrapper.GlobalServiceProvider;

        var windowManager = serviceProvider.GetRequiredService<IWindowManager>();

        var dialogResult = windowManager.ShowDialog(viewModel, screen);

        return (bool)dialogResult!;
    }
}
