namespace Application.Interfaces;


public interface ILocalizer
{
    public enum SupportedLanguages
    {
        English,
        Russian
    }


    string this[string key] { get; }

    void ChangeLanguage(SupportedLanguages language);
}
