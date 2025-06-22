using Cysharp.Threading.Tasks;

/// <summary>
/// Localization configuration data source
/// </summary>
public interface ILocalizationConfigSource
{
    /// <summary>
    /// Loads localization configuration
    /// </summary>
    /// <returns>Load result and configuration data</returns>
    UniTask<(bool success, LocalizationConfigData data)> LoadAsync();
    
    /// <summary>
    /// Source name for logging
    /// </summary>
    string SourceName { get; }
}