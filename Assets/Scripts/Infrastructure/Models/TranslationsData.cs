using System.Collections.Generic;
using Sirenix.OdinInspector;

[System.Serializable]
public class TranslationsDataCollection
{
    [DictionaryDrawerSettings(KeyLabel = "Key", ValueLabel = "Translation")]
    public Dictionary<string, string> translations = new Dictionary<string, string>();
}