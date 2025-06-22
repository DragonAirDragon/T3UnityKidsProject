using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerService
{
    private const float CUBE_SUPPORT_FACTOR = 0.7f;
    private const float CUBE_FALL_OFFSET = 0.3f;
    private const int MAX_FLOATING_REMOVAL_ITERATIONS = 10;

    private RectTransform _towerAreaRect;
    private readonly GameConfig _gameConfig;

    private readonly ICubeDataContainer _cubeDataContainer;
    private readonly ILoggerService _loggerService;
    private readonly IEffectsService _effectsService;
    private readonly CompositeCubeValidator _cubeValidator;

    public IReadOnlyList<CubeData> Cubes => _cubeDataContainer.Cubes;
    public event Action<List<int>> OnCubesNeedDestructionEffect;

    public TowerService(ICubeDataContainer cubeDataContainer, GameConfig gameConfig, RectTransform towerAreaRect, ILoggerService loggerService, IEffectsService effectsService, CompositeCubeValidator cubeValidator)
    {
        _cubeDataContainer = cubeDataContainer;
        _gameConfig = gameConfig;
        _towerAreaRect = towerAreaRect;
        _loggerService = loggerService;
        _effectsService = effectsService;
        _cubeValidator = cubeValidator;
    }
    
    public void PlaceCube(CubeColor color, Vector2 towerLocal, PointerEventData e)
    {
      
        // In DownLeft Anchor
        float localY = towerLocal.y + _towerAreaRect.rect.height * 0.5f;
        float localX = towerLocal.x + _towerAreaRect.rect.width * 0.5f;

        // Check if the cube is in the tower area
        if (!IsValidPlacement(localX, localY, color, e.position))
            return;



        CubeData cubeData;
        
        if (_cubeDataContainer.Count == 0)
        {
            cubeData = CreateCubeData(color, localX, _towerAreaRect.rect.width);
        }
        else
        {
            if (_gameConfig.isManualPlacement)
            {
               cubeData = CreateCubeData(color, localX, _towerAreaRect.rect.width);
                
            }
            else
            {
               cubeData = CreateCubeDataWithRandomOffset(color, _towerAreaRect.rect.width, _gameConfig.cubeWidth);
            }
        }
        
        cubeData.yOffset = CUBE_FALL_OFFSET;

        
        _cubeDataContainer.AddCube(cubeData);
        int cubeIndex = _cubeDataContainer.Count - 1; // Get index of added cube
        _loggerService?.LogCubePlaced(color, cubeIndex, cubeData.offset);
    }
    
    private bool IsValidPlacement(float localX, float localY, CubeColor color, Vector2 screenPosition)
    {
        var result = _cubeValidator.ValidatePlacement(localX, localY, color, screenPosition);
        if (!result.IsValid)
        {
            var effectDirection = result.EffectDirection ?? EffectDirection.Down;
            _effectsService?.PlayCubeWrongPlacementEffect(color, screenPosition, effectDirection);
            return false;
        }
        return true;
    }
    private CubeData CreateCubeData(CubeColor color, float localX, float towerAreaWidth)
    {
        float offset = localX / towerAreaWidth;
        return new CubeData
        {
            color = color,
            offset = offset,
            yOffset = 0f,
        };
    }

    private CubeData CreateCubeDataWithRandomOffset(CubeColor color, float towerAreaWidth, float cubeWidth)
    {
        float lastCubeOffset = _cubeDataContainer.Cubes.Last().offset;
        float maxOffsetDelta = (cubeWidth * 0.5f) / towerAreaWidth;
        float randomDelta = UnityEngine.Random.Range(-maxOffsetDelta, maxOffsetDelta);
        float offset = Mathf.Clamp01(lastCubeOffset + randomDelta);   
        return new CubeData
        {
            color = color,
            offset = offset,
            yOffset = 0f,
        };
    }

    public bool RemoveCubeAtIndex(int index)
    {
        if (!_cubeDataContainer.IsValidIndex(index))
            return false;
        
        var removedCube = _cubeDataContainer.GetCubeAt(index);

        _cubeDataContainer.ApplyGravityEffect(index);
        List<int> nonStableCubes = RemoveFloatingCubes(index);

        if (nonStableCubes != null && nonStableCubes.Count > 0)
        {
            OnCubesNeedDestructionEffect?.Invoke(nonStableCubes);
            _cubeDataContainer.RemoveCubesAt(nonStableCubes, false);
        }
        
        _cubeDataContainer.RemoveCubeAt(index);
        _loggerService?.LogCubeRemoved(removedCube.color, index);
        return true;
    }
    


    /// <summary>
    /// Calculates cube size
    /// </summary>
    public Vector2 GetCubeSize()
    {
        return new Vector2(_gameConfig.cubeWidth, _gameConfig.cubeHeight);
    }

    private List<int> RemoveFloatingCubes(int removedIndex)
    {
        if (_cubeDataContainer.Count <= 1) {
            return null;
        }
        if(removedIndex == 0)
        {
            return null;
        }
        
        List<int> nonStableCubes = new List<int>();
        
        int stableIndex = removedIndex - 1;
        
        if (stableIndex >= _cubeDataContainer.Count)
            return nonStableCubes;
            
        CubeData stableCube = _cubeDataContainer.GetCubeAt(stableIndex);
        float stableCubeOffset = stableCube.offset;
        float maxOffsetDifference = GetMaxStabilityDistance();
        
        for (int i = 0; i < _cubeDataContainer.Count; i++)
        {
            if (i == removedIndex || i < removedIndex) continue;
            
            var cube = _cubeDataContainer.GetCubeAt(i);
            if (cube.offset > stableCubeOffset + maxOffsetDifference || 
                cube.offset < stableCubeOffset - maxOffsetDifference)
            {
                nonStableCubes.Add(i);
            }
        }
        
        return nonStableCubes;
    }

    private float GetMaxStabilityDistance()
    {
        float cubeWidthRelative = _gameConfig.cubeWidth / _towerAreaRect.rect.width;
        return cubeWidthRelative * CUBE_SUPPORT_FACTOR;
    }


    public Vector2 GetCubePosition(CubeData cubeData, RectTransform cubesContainer, int cubeIndex)
    {
        float x = cubeData.offset * cubesContainer.rect.width;
        float y = cubeIndex * _gameConfig.cubeHeight;
        return new Vector2(x, y);
    }

}