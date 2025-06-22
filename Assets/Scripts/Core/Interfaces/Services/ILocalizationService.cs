using System;

/// <summary>
/// Interface for localization service
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Event triggered when language changes
    /// </summary>
    event Action OnLanguageChanged;
    
    /// <summary>
    /// Get localized text by key
    /// </summary>
    /// <param name="key">Localization key</param>
    /// <returns>Localized text</returns>
    string GetText(string key);
    
    /// <summary>
    /// Set current language
    /// </summary>
    /// <param name="language">Language to set</param>
    void SetLanguage(Language language);
    
    /// <summary>
    /// Get current language
    /// </summary>
    Language CurrentLanguage { get; }
}

