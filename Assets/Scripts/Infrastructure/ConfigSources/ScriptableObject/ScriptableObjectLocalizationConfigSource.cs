using Cysharp.Threading.Tasks;

/// <summary>
/// Localization configuration source from ScriptableObject
/// </summary>
public class ScriptableObjectLocalizationConfigSource : ILocalizationConfigSource
{
    private readonly LocalizationConfig _scriptableObject;

    public string SourceName => "ScriptableObject";

    public ScriptableObjectLocalizationConfigSource(LocalizationConfig scriptableObject)
    {
        _scriptableObject = scriptableObject;
    }

    public UniTask<(bool success, LocalizationConfigData data)> LoadAsync()
    {
        if (_scriptableObject == null || _scriptableObject.translations == null)
        {
            return UniTask.FromResult((false, (LocalizationConfigData)null));
        }

        // Map ScriptableObject to DTO
        var data = new LocalizationConfigData();
        
        foreach (var kvp in _scriptableObject.translations)
        {
            data.translations[kvp.Key] = kvp.Value;
        }

        return UniTask.FromResult((data.IsValid(), data));
    }
}