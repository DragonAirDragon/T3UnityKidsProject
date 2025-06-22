/// <summary>
/// Service for obtaining animations configuration
/// </summary>
public interface IAnimationsConfigService
{
    /// <summary>
    /// Animations configuration data (guaranteed to be valid)
    /// </summary>
    AnimationsConfigData Config { get; }
}