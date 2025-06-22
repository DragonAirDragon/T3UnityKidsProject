using UnityEngine;

public class CubeOffsetValidator : ICubeValidator
{
    private readonly ICubeDataContainer _cubeDataContainer;
    private readonly GameConfig _gameConfig;
    private readonly RectTransform _towerAreaRect;
    private readonly ILoggerService _loggerService;

    public CubeOffsetValidator(ICubeDataContainer cubeDataContainer, GameConfig gameConfig, RectTransform towerAreaRect, ILoggerService loggerService)
    {
        _cubeDataContainer = cubeDataContainer;
        _gameConfig = gameConfig;
        _towerAreaRect = towerAreaRect;
        _loggerService = loggerService;
    }

    public ValidationResult ValidatePlacement(float localX, float localY, CubeColor color, Vector2 screenPosition)
    {
        if (CheckMaxCubeOffset(localX, out EffectDirection? effectDirection))
            return ValidationResult.Valid(effectDirection);
        _loggerService?.LogCubeIncorrectOffset(color);
        return ValidationResult.Invalid(effectDirection);
    }

    private bool CheckMaxCubeOffset(float localX, out EffectDirection? effectDirection)
    {
        effectDirection = null;
        
        if (_cubeDataContainer.Count == 0)
            return true;
        
        float maxOffset = 0.5f * _gameConfig.cubeWidth;
        float previousCubePositionX = GetCubePositionX(_cubeDataContainer.Cubes[_cubeDataContainer.Count - 1], _towerAreaRect);
        
        if (localX > maxOffset + previousCubePositionX)
        {
            effectDirection = EffectDirection.DownRight;
            return false;
        }
        
        if (localX < previousCubePositionX - maxOffset)
        {
            effectDirection = EffectDirection.DownLeft;
            return false;
        }
        
        return true;
    }

    private float GetCubePositionX(CubeData cubeData, RectTransform cubesContainer)
    {
        return cubeData.offset * cubesContainer.rect.width;
    }
}