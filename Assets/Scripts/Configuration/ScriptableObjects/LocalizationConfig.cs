using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Config for LocalizationService
/// </summary>
[CreateAssetMenu(fileName = "LocalizationConfig", menuName = "Configs/Localization Config")]
public class LocalizationConfig : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel = "Language", ValueLabel = "Translations")]
    [InfoBox("Dictionary of translations for different languages. Each language contains a collection of key-value pairs.")]
    public Dictionary<Language, TranslationsDataCollection> translations = new();
}