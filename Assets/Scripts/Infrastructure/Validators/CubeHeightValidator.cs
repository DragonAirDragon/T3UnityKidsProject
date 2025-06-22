using UnityEngine;

public class CubeHeightValidator : ICubeValidator
{
    private readonly ICubeDataContainer _cubeDataContainer;
    private readonly GameConfig _gameConfig;
    private readonly ILoggerService _loggerService;

    public CubeHeightValidator(ICubeDataContainer cubeDataContainer, GameConfig gameConfig, ILoggerService loggerService)
    {
        _cubeDataContainer = cubeDataContainer;
        _gameConfig = gameConfig;
        _loggerService = loggerService;
    }

    public ValidationResult ValidatePlacement(float localX, float localY, CubeColor color, Vector2 screenPosition)
    {
        if (CheckHeight(localY))
            return ValidationResult.Valid();
        _loggerService?.LogCubeIncorrectHeight(color);
        return ValidationResult.Invalid(EffectDirection.Down);
    }

    private bool CheckHeight(float localY)
    {
        float currentTowerHeight = _cubeDataContainer.Count * _gameConfig.cubeHeight;
        bool isValid = currentTowerHeight <= localY + _gameConfig.cubeHeight * 0.5f;
        return isValid;
    }
}