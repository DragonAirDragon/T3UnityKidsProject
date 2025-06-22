using UnityEngine;

/// <summary>
/// Service responsible for playing effects
/// </summary>
public sealed class EffectsService : IEffectsService
{
    private readonly CubeEffect _cubeEffectPrefab;
    private readonly RectTransform _effectsArea;
    private readonly GameConfig _gameConfig;
    private readonly IAnimationsConfigService _animationsConfigService;

    public EffectsService(CubeEffect cubeEffectPrefab, RectTransform effectsArea, GameConfig gameConfig, IAnimationsConfigService animationsConfigService)
    {
        _cubeEffectPrefab = cubeEffectPrefab;
        _effectsArea = effectsArea;
        _gameConfig = gameConfig;
        _animationsConfigService = animationsConfigService;
    }


    /// <summary>
    /// Need for incorrect zone effect
    /// </summary>
    /// <param name="color">Enum of the cube color</param>
    /// <param name="screenPosition">Screen position of the cube</param>
    public void PlayCubeWrongZoneEffect(CubeColor color, Vector2 screenPosition)
    {
        Vector2 localPosition = ConvertScreenToLocalPosition(screenPosition, _effectsArea);
        var effectInstance = CreateCubeEffect(color, localPosition, _effectsArea);
        effectInstance.PlayDissolveEffect(_animationsConfigService.Config.cubeDissolveDuration);
    }
    /// <summary>
    /// Play cube wrong placement in tower effect
    /// </summary>
    /// <param name="color">Enum of the cube color</param>
    /// <param name="screenPosition">Screen position of the cube</param>
    /// <param name="direction">Direction of the cube</param>
    public void PlayCubeWrongPlacementEffect(CubeColor color, Vector2 screenPosition, EffectDirection direction)
    {
        Vector2 localPosition = ConvertScreenToLocalPosition(screenPosition, _effectsArea);
        var effectInstance = CreateCubeEffect(color, localPosition, _effectsArea);
        effectInstance.PlayDissolveEffectWithFall(direction, _animationsConfigService.Config.cubeFallHorizontalDuration, _animationsConfigService.Config.cubeFallHorizontalDistance);
    }
    /// <summary>
    /// Play cube successful place in hole effect
    /// </summary>
    /// <param name="color">Enum of the cube color</param>
    /// <param name="screenPosition">Screen position of the cube</param>
    /// <param name="direction">Direction of the cube</param>
    /// <param name="parent">Parent of the cube</param>
    public void PlayCubeSuccessfulRemovalEffect(CubeColor color, Vector2 screenPosition, EffectDirection direction, RectTransform parent)
    {
        Vector2 localPosition = ConvertScreenToLocalPosition(screenPosition, parent);
        var effectInstance = CreateCubeEffect(color, localPosition, parent);
        effectInstance.PlayFallEffect(direction, _animationsConfigService.Config.cubeFallToHoleDistance, _animationsConfigService.Config.cubeFallToHoleDuration);
    }

    /// <summary>
    /// Convert screen position to local position for current parent
    /// </summary>
    /// <param name="screenPosition">Screen position</param>
    /// <param name="parent">Parent of the cube</param>
    /// <returns>Local position</returns>
    private Vector2 ConvertScreenToLocalPosition(Vector2 screenPosition, RectTransform parent)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent, screenPosition, Camera.main, out var localPosition);
        return localPosition;
    }

    private CubeEffect CreateCubeEffect(CubeColor color, Vector2 localPosition, RectTransform parent)
    {
        var effectInstance = Object.Instantiate(_cubeEffectPrefab, parent);
        var cubeSprite = _gameConfig.GetSpriteForCubeColor(color);
        effectInstance.Setup(cubeSprite, localPosition);
        
        return effectInstance;
    }
}