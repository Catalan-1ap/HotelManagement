using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class ShellViewModel : Conductor<TabScreen>.Collection.OneActive
{
    private readonly MainTabViewModel _mainTab;


    public ShellViewModel(MainTabViewModel mainTab)
    {
        _mainTab = mainTab;
        RestoreMainTab();
    }


    public void RestoreMainTab() => ActivateItem(_mainTab);


    public void RemoveTab() => CloseItem(ActiveItem);
}
