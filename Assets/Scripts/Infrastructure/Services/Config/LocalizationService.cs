using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Service responsible for managing localization and providing text translations
/// </summary>
public class LocalizationService : ILocalizationService
{
    private Language _currentLanguage = Language.Russian;
    private Dictionary<string, string> _currentTranslations;
    public event Action OnLanguageChanged;
    public Language CurrentLanguage => _currentLanguage;
    private readonly ILocalizationConfigService _configService;
    public LocalizationService(ILocalizationConfigService configService)
    {
        _configService = configService;
        _configService.OnConfigUpdated += UpdateCurrentTranslations;
        
        // Инициализируем переводы при создании сервиса
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

        _currentTranslations = _configService.Config.translations.TryGetValue(_currentLanguage, out var translations) 
        ? translations.translations : new Dictionary<string, string>();
    }
    
} 