using System.Collections.Generic;
using FluentValidation;
using PropertyChanged;
using Stylet;


namespace Wpf.Common;


public class InputScreenBase : Screen
{
    public InputScreenBase() => Validator = new FluentModelValidator<InputScreenBase>(new InlineValidator<InputScreenBase>());


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
