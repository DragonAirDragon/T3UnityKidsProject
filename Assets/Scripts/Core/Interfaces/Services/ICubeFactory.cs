using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface for cube factory service
/// </summary>
public interface ICubeFactory
{
    /// <summary>
    /// Create inventory cube
    /// </summary>
    /// <param name="color">Cube color</param>
    /// <param name="scrollRect">Parent scroll rect</param>
    /// <param name="parent">Parent transform</param>
    /// <returns>Created cube GameObject</returns>
    GameObject CreateInventoryCube(CubeColor color, ScrollRect scrollRect, Transform parent);
    
    /// <summary>
    /// Create tower cube
    /// </summary>
    /// <param name="cubeData">Cube data</param>
    /// <param name="parent">Parent transform</param>
    /// <returns>Created cube GameObject</returns>
    GameObject CreateTowerCube(CubeData cubeData, Transform parent);
} 