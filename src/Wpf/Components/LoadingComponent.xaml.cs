using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Wpf.Common;


namespace Wpf.Components;


[ContentProperty("AdditionalContent")]
public partial class LoadingComponent : UserControl
{
    public static readonly DependencyProperty LoadingControllerProperty = DependencyProperty.Register(
        "LoadingController",
        typeof(LoadingController),
        typeof(LoadingComponent),
        new(default(LoadingController)));


    public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
        "AdditionalContent",
        typeof(object),
        typeof(LoadingComponent),
        new(default(object)));


    public LoadingComponent() => InitializeComponent();

    public LoadingController LoadingController
    {
        get => (LoadingController)GetValue(LoadingControllerProperty);
        set => SetValue(LoadingControllerProperty, value);
    }

    public object AdditionalContent
    {
        get => GetValue(AdditionalContentProperty);
        set => SetValue(AdditionalContentProperty, value);
    }
}
