using Cysharp.Threading.Tasks;

/// <summary>
/// Animations configuration data source
/// </summary>
public interface IAnimationsConfigSource
{
    /// <summary>
    /// Loads animations configuration
    /// </summary>
    /// <returns>Load result and configuration data</returns>
    UniTask<(bool success, AnimationsConfigData data)> LoadAsync();
    
    /// <summary>
    /// Source name for logging
    /// </summary>
    string SourceName { get; }
}