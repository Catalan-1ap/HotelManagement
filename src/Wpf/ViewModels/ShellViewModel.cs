using System;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class ShellViewModel : Conductor<TabScreen>.Collection.OneActive
{
    private readonly IServiceProvider _serviceProvider;


    public ShellViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        RestoreMainTab();
    }


    public void RestoreMainTab() => ActivateItem(_serviceProvider.GetRequiredService<MainTabViewModel>());


    public void RemoveTab() => CloseItem(ActiveItem);
}
