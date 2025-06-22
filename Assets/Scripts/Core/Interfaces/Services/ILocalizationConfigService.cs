/// <summary>
/// Service for obtaining localization configuration
/// </summary>
public interface ILocalizationConfigService
{
    /// <summary>
    /// Localization configuration data (guaranteed to be valid)
    /// </summary>
    LocalizationConfigData Config { get; }

    /// <summary>
    /// Configuration change event (e.g. when updating from remote source)
    /// </summary>
    System.Action OnConfigUpdated { get; set; }
}