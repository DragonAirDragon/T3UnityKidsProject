using Cysharp.Threading.Tasks;

/// <summary>
/// Inventory configuration data source
/// </summary>
public interface IInventoryConfigSource
{
    /// <summary>
    /// Loads inventory configuration
    /// </summary>
    /// <returns>Load result and configuration data</returns>
    UniTask<(bool success, InventoryConfigData data)> LoadAsync();
    
    /// <summary>
    /// Source name for logging
    /// </summary>
    string SourceName { get; }
}