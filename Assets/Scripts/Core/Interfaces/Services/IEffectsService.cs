using UnityEngine;

/// <summary>
/// Interface for visual effects service
/// </summary>
public interface IEffectsService
{
    /// <summary>
    /// Play effect when cube is placed in wrong zone
    /// </summary>
    /// <param name="color">Cube color</param>
    /// <param name="screenPosition">Screen position for effect</param>
    void PlayCubeWrongZoneEffect(CubeColor color, Vector2 screenPosition);
    
    /// <summary>
    /// Play effect when cube is placed with wrong placement
    /// </summary>
    /// <param name="color">Cube color</param>
    /// <param name="screenPosition">Screen position for effect</param>
    /// <param name="direction">Effect direction</param>
    void PlayCubeWrongPlacementEffect(CubeColor color, Vector2 screenPosition, EffectDirection direction);
    
    /// <summary>
    /// Play effect when cube is successfully removed
    /// </summary>
    /// <param name="color">Cube color</param>
    /// <param name="screenPosition">Screen position for effect</param>
    /// <param name="direction">Effect direction</param>
    /// <param name="parent">Parent transform for effect</param>
    void PlayCubeSuccessfulRemovalEffect(CubeColor color, Vector2 screenPosition, EffectDirection direction, RectTransform parent);
    
}