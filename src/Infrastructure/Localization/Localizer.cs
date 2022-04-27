using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Application.Interfaces;


namespace Infrastructure.Localization;


public sealed class Localizer : ILocalizer
{
    private static readonly Dictionary<ILocalizer.SupportedLanguages, CultureInfo> Cultures = new()
    {
        [ILocalizer.SupportedLanguages.Russian] = new("ru-RU")
    };


    private ResourceManager _manager = null!;


    public Localizer() => LoadLanguage();


    public string this[string key]
    {
        get
        {
            var value = _manager.GetString(key);

            if (string.IsNullOrEmpty(value))
                value = $"Non localized key \"{key}\"";

            return value;
        }
    }


    public void ChangeLanguage(ILocalizer.SupportedLanguages language)
    {
        CultureInfo.CurrentUICulture = Cultures[language];
        LoadLanguage();
    }


    private void LoadLanguage() => _manager = new(typeof(Language));
}
