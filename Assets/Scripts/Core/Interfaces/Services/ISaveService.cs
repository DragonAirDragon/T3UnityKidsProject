using System.Collections.Generic;

/// <summary>
/// Interface for save/load operations
/// </summary>
public interface ISaveService
{
    /// <summary>
    /// Save tower data
    /// </summary>
    /// <param name="cubes">Cubes to save</param>
    void SaveTowerData(List<CubeData> cubes);
    
    /// <summary>
    /// Load tower data
    /// </summary>
    /// <returns>Loaded cubes data</returns>
    List<CubeData> LoadTowerData();
    
    /// <summary>
    /// Check if saved data exists
    /// </summary>
    /// <returns>True if saved data exists</returns>
    bool HasSavedData();
    
    /// <summary>
    /// Clear all saved data
    /// </summary>
    void ClearSavedData();
} 