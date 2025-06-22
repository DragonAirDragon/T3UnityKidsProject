using System;
using System.Linq;

/// <summary>
/// Pure inventory configuration data object (without Unity dependencies)
/// </summary>
[Serializable]
public class InventoryConfigData
{
    public CubeColor[] availableCubeColors;

    /// <summary>
    /// Configuration data validation
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return availableCubeColors != null && 
               availableCubeColors.Length > 0 &&
               availableCubeColors.Length == availableCubeColors.Distinct().Count(); // Check uniqueness
    }

    /// <summary>
    /// Check if color is available in inventory
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsColorAvailable(CubeColor color)
    {
        return availableCubeColors?.Contains(color) == true;
    }

    /// <summary>
    /// Get count of available colors
    /// </summary>
    /// <returns></returns>
    public int GetAvailableColorsCount()
    {
        return availableCubeColors?.Length ?? 0;
    }
}