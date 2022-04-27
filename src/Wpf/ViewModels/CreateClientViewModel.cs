using Stylet;
using Wpf.Common;


namespace Wpf.ViewModels;


public sealed class CreateClientViewModel : InputScreenBase
{
    public CreateClientViewModel(IModelValidator<CreateClientViewModel> validator)
    {
        Validator = validator;
        Validate();
    }


    public string Passport { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
}
