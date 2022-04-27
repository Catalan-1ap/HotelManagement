using System.Collections.Generic;
using PropertyChanged;
using Stylet;


namespace Wpf.Common;


public class InputScreenBase : Screen
{
    public virtual bool CanAccept => HasErrors == false;


    public void Accept()
    {
        if (Validate() == false)
            return;

        RequestClose(dialogResult: true);
    }


    public void Reject() => RequestClose(dialogResult: false);


    [SuppressPropertyChangedWarnings]
    protected override void OnValidationStateChanged(IEnumerable<string> changedProperties)
    {
        base.OnValidationStateChanged(changedProperties);

        NotifyOfPropertyChange(() => CanAccept);
    }
}
