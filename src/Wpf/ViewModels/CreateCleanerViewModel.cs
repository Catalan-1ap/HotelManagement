using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class CreateCleanerViewModel : InputScreenBase
{
    public CreateCleanerViewModel(IModelValidator<CreateCleanerViewModel> validator)
    {
        Validator = validator;
        Validate();
    }


    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
}
