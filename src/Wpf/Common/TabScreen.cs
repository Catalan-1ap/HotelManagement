using System.Windows.Input;
using Stylet;


namespace Wpf.Common;


public class TabScreen : Screen
{
    protected TabScreen(string displayName) => DisplayName = displayName;


    protected override void OnViewLoaded()
    {
        View.Focusable = true;
        Keyboard.Focus(View);
    }


    protected void ActivateOtherTab(TabScreen tabScreen)
    {
        if (Parent is not Conductor<TabScreen>.Collection.OneActive parent)
            throw new($"Parent of {GetType()} must be of type {nameof(Conductor<TabScreen>.Collection.OneActive)} but found {Parent.GetType()}");

        parent.ActivateItem(tabScreen);
    }
}
