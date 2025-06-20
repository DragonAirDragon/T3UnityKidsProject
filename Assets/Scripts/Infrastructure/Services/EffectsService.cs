using UnityEngine;

public class EffectsService
{
    private readonly CubeEffect _cubeEffectPrefab;
    private readonly RectTransform _effectsArea;
    private readonly GameConfig _gameConfig;

    public EffectsService(CubeEffect cubeEffectPrefab, RectTransform effectsArea, GameConfig gameConfig)
    {
        _cubeEffectPrefab = cubeEffectPrefab;
        _effectsArea = effectsArea;
        _gameConfig = gameConfig;
    }

    public void PlayCubeDissolveEffect(CubeColor color, Vector2 screenPosition)
    {
        Vector2 localPosition = ConvertScreenToLocalPosition(screenPosition, _effectsArea);
        var effectInstance = CreateCubeEffect(color, localPosition, _effectsArea);
        effectInstance.PlayDissolveEffect();
    }

    public void PlayCubeDissolveEffectWithFall(CubeColor color, Vector2 screenPosition, EffectDirection direction)
    {
        Vector2 localPosition = ConvertScreenToLocalPosition(screenPosition, _effectsArea);
        var effectInstance = CreateCubeEffect(color, localPosition, _effectsArea);
        effectInstance.PlayDissolveEffectWithFall(direction);
    }
    
    public void PlayCubeFallEffect(CubeColor color, Vector2 screenPosition, EffectDirection direction, RectTransform parent, float fallDistance)
    {
        Vector2 localPosition = ConvertScreenToLocalPosition(screenPosition, parent);
        var effectInstance = CreateCubeEffect(color, localPosition, parent);
        effectInstance.PlayFallEffect(direction, fallDistance, 2f);
    }
    
    /// <summary>
    /// Конвертирует экранные координаты в локальные относительно области эффектов
    /// </summary>
    private Vector2 ConvertScreenToLocalPosition(Vector2 screenPosition, RectTransform parent)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent, screenPosition, Camera.main, out var localPosition);
        return localPosition;
    }
    /// <summary>
    /// Создает экземпляр эффекта куба
    /// </summary>
    private CubeEffect CreateCubeEffect(CubeColor color, Vector2 localPosition, RectTransform parent)
    {
        var effectInstance = Object.Instantiate(_cubeEffectPrefab, parent);
        var cubeSprite = _gameConfig.GetSpriteForCubeColor(color);
        effectInstance.Setup(cubeSprite, localPosition);
        
        return effectInstance;
    }
}