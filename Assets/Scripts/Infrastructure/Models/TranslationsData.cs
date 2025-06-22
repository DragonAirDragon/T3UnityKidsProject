using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

/// <summary>
/// Serializable class for translations data
/// </summary>
[Serializable]
public class TranslationsDataCollection
{
    [DictionaryDrawerSettings(KeyLabel = "Key", ValueLabel = "Translation")]
    public Dictionary<string, string> translations = new Dictionary<string, string>();
}