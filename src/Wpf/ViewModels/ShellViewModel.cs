using System.Linq;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class ShellViewModel : Conductor<TabScreen>.Collection.OneActive
{
    public bool IsAtLeastOneTabExists => Items.Any();
    public bool IsNoTabs => !IsAtLeastOneTabExists;

    public bool CanShowEditCleaners => CanShow<ManageCleanersViewModel>();
    public bool CanShowEditClients => CanShow<ManageClientsViewModel>();


    public void ShowEditCleaners() => OpenTab<ManageCleanersViewModel>();

    public void ShowEditClients() => OpenTab<ManageClientsViewModel>();


    public void RemoveTab()
    {
        CloseItem(ActiveItem);
        NotifyOfTabsChanges();
    }


    public void Reload(object sender, SelectionChangedEventArgs e)
    {
        if (e.OriginalSource is TabControl)
            (ActiveItem as ILoadable)?.Load();
    }


    private bool CanShow<TTabViewModel>() where TTabViewModel : TabScreen => !Items.Any(screen => screen is TTabViewModel);


    private void OpenTab<TTabViewModel>()
        where TTabViewModel : TabScreen
    {
        var viewModel = Bootstrapper.GlobalServiceProvider.GetRequiredService<TTabViewModel>();

        ActivateItem(viewModel);
        NotifyOfTabsChanges();
    }


    private void NotifyOfTabsChanges() => Refresh();
}
