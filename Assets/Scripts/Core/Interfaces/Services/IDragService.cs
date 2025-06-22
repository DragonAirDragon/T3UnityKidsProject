using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Interface for drag and drop service
/// </summary>
public interface IDragService
{
    /// <summary>
    /// Set ghost cube visibility
    /// </summary>
    /// <param name="active">True to show ghost cube</param>
    void SetGhostCubeActive(bool active);
    
    /// <summary>
    /// Set ghost cube sprite
    /// </summary>
    /// <param name="color">Cube color</param>
    void SetGhostCubeSprite(CubeColor color);
    
    /// <summary>
    /// Move ghost cube to follow pointer
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    void MoveGhost(PointerEventData eventData);
    
    /// <summary>
    /// Drop cube to tower
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    /// <param name="color">Cube color</param>
    void DropCubeToTower(PointerEventData eventData, CubeColor color);
    
    /// <summary>
    /// Drop cube to hole (remove from tower)
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    /// <param name="color">Cube color</param>
    /// <param name="cubeIndex">Index of cube in tower</param>
    void DropCubeToHole(PointerEventData eventData, CubeColor color, int cubeIndex);
}