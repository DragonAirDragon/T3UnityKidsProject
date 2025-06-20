using System;

public interface ILocalizationService
{
    event Action OnLanguageChanged;
    
    string GetText(string key);
    void SetLanguage(Language language);
    Language CurrentLanguage { get; }
}

