using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationService : ILocalizationService
{
    private Language _currentLanguage = Language.Russian;
    private Dictionary<string, string> _currentTranslations;
    public event Action OnLanguageChanged;
    public Language CurrentLanguage => _currentLanguage;
    
    private readonly LocalizationConfig _config;

    public LocalizationService(LocalizationConfig config)
    {
        _config = config;
        UpdateCurrentTranslations();
    }

    public string GetText(string key)
    {
        if (string.IsNullOrEmpty(key)) return key;
        
        if (_currentTranslations?.TryGetValue(key, out var translation) == true)
        {
            return !string.IsNullOrEmpty(translation) ? translation : key;
        }
        
        Debug.LogWarning($"Translation not found for key: '{key}', language: {_currentLanguage}");
        return key;
    }

    public void SetLanguage(Language language)
    {
        if (_currentLanguage != language)
        {
            _currentLanguage = language;
            UpdateCurrentTranslations();
            OnLanguageChanged?.Invoke();
        }
    }

    public void UpdateCurrentTranslations(){

        _currentTranslations = _config.translations.TryGetValue(_currentLanguage, out var translations) 
        ? translations.translations : new Dictionary<string, string>();
    }
    
} 