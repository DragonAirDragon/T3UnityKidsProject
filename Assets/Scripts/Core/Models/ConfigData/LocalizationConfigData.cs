using System;
using System.Collections.Generic;

/// <summary>
/// Pure localization configuration data object (without Unity dependencies)
/// </summary>
[Serializable]
public class LocalizationConfigData
{
    public Dictionary<Language, TranslationsDataCollection> translations = new Dictionary<Language, TranslationsDataCollection>();

    /// <summary>
    /// Configuration data validation
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        if (translations == null || translations.Count == 0)
            return false;

        // Check that there is at least one language with translations
        foreach (var kvp in translations)
        {
            if (kvp.Value?.translations != null && kvp.Value.translations.Count > 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Get translations for specific language
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    public Dictionary<string, string> GetTranslationsForLanguage(Language language)
    {
        return translations.TryGetValue(language, out var collection) 
            ? collection.translations ?? new Dictionary<string, string>()
            : new Dictionary<string, string>();
    }

    /// <summary>
    /// Add or update translations for language
    /// </summary>
    /// <param name="language"></param>
    /// <param name="languageTranslations"></param>
    public void SetTranslationsForLanguage(Language language, Dictionary<string, string> languageTranslations)
    {
        if (!translations.ContainsKey(language))
        {
            translations[language] = new TranslationsDataCollection();
        }
        translations[language].translations = languageTranslations ?? new Dictionary<string, string>();
    }
}