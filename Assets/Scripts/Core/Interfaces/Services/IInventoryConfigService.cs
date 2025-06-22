/// <summary>
/// Service for obtaining inventory configuration
/// </summary>
public interface IInventoryConfigService
{
    /// <summary>
    /// Inventory configuration data (guaranteed to be valid)
    /// </summary>
    InventoryConfigData Config { get; }

    /// <summary>
    /// Checks if configuration was loaded from external source
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// Configuration change event (for future remote sources)
    /// </summary>
    System.Action OnConfigUpdated { get; set; }
}