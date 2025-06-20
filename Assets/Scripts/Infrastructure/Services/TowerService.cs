using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class TowerService
{
    private List<CubeData> _cubes = new();
    private readonly GameConfig _gameConfig;
    private readonly ILoggerService _loggerService;

    public event Action<List<CubeData>> OnListChanged;
    public IReadOnlyList<CubeData> Cubes => _cubes;
    private RectTransform _cubesContainer;
    private EffectsService _effectsService;

    public TowerService(GameConfig gameConfig, RectTransform cubesContainer, ILoggerService loggerService, EffectsService effectsService)
    {
        _gameConfig = gameConfig;
        _cubesContainer = cubesContainer;
        _loggerService = loggerService;
        _effectsService = effectsService;
    }

    /// <summary>
    /// Загружает башню из переданных данных
    /// </summary>
    public void LoadTowerData(List<CubeData> savedCubes)
    {
        if (savedCubes?.Count > 0)
        {
            _cubes.Clear();
            _cubes.AddRange(savedCubes);
            OnListChanged?.Invoke(_cubes);
            Debug.Log($"TowerService: Загружено {_cubes.Count} кубов");
        }
    }

    /// <summary>
    /// Размещает куб в башне с вычисленным offset'ом
    /// </summary>
    /// <param name="color">Цвет куба</param>
    /// <param name="localX">Локальная X-координата в области башни</param>
    /// <param name="localY">Локальная Y-координата в области башни</param>
    /// <param name="towerAreaWidth">Ширина области башни в локальных координатах</param>
    /// <param name="towerArea">Контейнер кубов</param>
    public void PlaceCube(CubeColor color, Vector2 towerLocal, RectTransform towerArea, PointerEventData e)
    {
        
        float towerAreaWidth = towerArea.rect.width;
        float localY = towerLocal.y + towerArea.rect.height * 0.5f;
        float localX = towerLocal.x + towerArea.rect.width * 0.5f;
        if(!CheckMaxCubeOffset(localX, towerArea, out bool isRight))
        {
            Debug.Log("PlaceCube: Отклонено CheckMaxCubeOffset");
            _loggerService?.LogCubeIncorrectOffset(color);
            _effectsService?.PlayCubeDissolveEffectWithFall(color, e.position, isRight ? EffectDirection.DownRight : EffectDirection.DownLeft);
            return;
        }
        if(!CheckHeight(localY))
        {
            Debug.Log("PlaceCube: Отклонено CheckHeight");
            _loggerService?.LogCubeIncorrectHeight(color);
            _effectsService?.PlayCubeDissolveEffectWithFall(color, e.position, EffectDirection.Down);
            return;
        }
        
        float offset = localX / towerAreaWidth;
        var cubeData = new CubeData
        {
            color = color,
            order = _cubes.Count, // Порядок = текущий размер списка
            offset = offset
        };
        _cubes.Add(cubeData);
        Debug.Log($"PlaceCube: Успешно размещен куб Color={color}, Order={cubeData.order}, Offset={offset:F3}");
        
        _loggerService?.LogCubePlaced(color, cubeData.order, offset);
        OnListChanged?.Invoke(_cubes);
    }

    private bool CheckHeight(float localY)
    {
        float currentTowerHeight = _cubes.Count * _gameConfig.cubeHeight;
        if (currentTowerHeight > localY + _gameConfig.cubeHeight * 0.5f)
        {
            Debug.Log($"CheckHeight: Куб ниже башни. currentTowerHeight={currentTowerHeight}, localY={localY}");
            return false;
        }
        
        return true;
    }

    private bool CheckMaxCubeOffset(float localX, RectTransform cubesContainer, out bool isRight)
    {
        if(_cubes.Count == 0)
        {
            isRight = false;
            return true;
        }
        
        float maxOffset = 0.5f * _gameConfig.cubeWidth;
        float previousCubePositionX = GetCubePositionX(_cubes.Last(), cubesContainer);
        
        // Проверяем направление нарушения
        if(localX > maxOffset + previousCubePositionX)
        {
            isRight = true; // Слишком далеко вправо
            return false;
        }
        else if(localX < previousCubePositionX - maxOffset)
        {
            isRight = false; // Слишком далеко влево
            return false;
        }
        
        isRight = false; // Не важно, куб в пределах допустимого
        return true;
    }


    public float GetCubePositionX(CubeData cubeData, RectTransform cubesContainer)
    {
        return cubeData.offset * cubesContainer.rect.width;
    }

    /// <summary>
    /// Вычисляет полную позицию куба в локальных координатах контейнера
    /// </summary>
    public Vector2 GetCubePosition(CubeData cubeData, RectTransform cubesContainer)
    {
        float x = cubeData.offset * cubesContainer.rect.width;
        float y = cubeData.order * _gameConfig.cubeHeight;
        return new Vector2(x, y);
    }

    /// <summary>
    /// Вычисляет размер куба
    /// </summary>
    public Vector2 GetCubeSize()
    {
        return new Vector2(_gameConfig.cubeWidth, _gameConfig.cubeHeight);
    }

    /// <summary>
    /// Убирает последний куб из башни
    /// </summary>
    public bool RemoveTopCube()
    {
        if (_cubes.Count == 0) return false;
        
        var removedCube = _cubes[_cubes.Count - 1];
        _cubes.RemoveAt(_cubes.Count - 1);
        
        Debug.Log($"RemoveTopCube: Color={removedCube.color}, Order={removedCube.order}");
        _loggerService?.LogCubeRemoved(removedCube.color, removedCube.order);
        OnListChanged?.Invoke(_cubes);
        return true;
    }

    /// <summary>
    /// Убирает куб из башни по индексу
    /// </summary>
    public bool RemoveCubeAtIndex(int index)
    {
        if (index < 0 || index >= _cubes.Count)
        {
            Debug.LogWarning($"RemoveCubeAtIndex: Некорректный индекс {index}. Количество кубов: {_cubes.Count}");
            return false;
        }
        
        var removedCube = _cubes[index];
        _cubes.RemoveAt(index);
        
        // Пересчитываем порядковые номера для всех кубов после удаленного
        for (int i = index; i < _cubes.Count; i++)
        {
            var cube = _cubes[i];
            cube.order = i;
            _cubes[i] = cube;
        }
        
        _loggerService?.LogCubeRemoved(removedCube.color, removedCube.order);
        
        // Проверяем и удаляем "висящие" кубы после удаления
        RemoveFloatingCubes();
        
        Debug.Log($"RemoveCubeAtIndex: Удален куб Color={removedCube.color}, Index={index}");
        OnListChanged?.Invoke(_cubes);
        return true;
    }

    /// <summary>
    /// Удаляет кубы, которые не имеют достаточной опоры снизу
    /// </summary>
    private void RemoveFloatingCubes()
    {
        if (_cubes.Count <= 1) return; // Первый куб всегда устойчив
        
        List<int> cubesToRemove = new List<int>();
        
        // Проверяем все кубы начиная со второго (индекс 1)
        for (int i = 1; i < _cubes.Count; i++)
        {
            var currentCube = _cubes[i];
            var lowerCube = _cubes[i - 1];
            
            // Проверяем, пересекаются ли кубы по горизонтали
            // offset - это относительная позиция от 0 до 1
            float offsetDifference = Mathf.Abs(currentCube.offset - lowerCube.offset);
            float cubeWidthRelative = _gameConfig.cubeWidth / _cubesContainer.rect.width;
            float maxOffsetDifference = cubeWidthRelative * 0.7f;
            
            if (offsetDifference > maxOffsetDifference)
            {
                Debug.Log($"RemoveFloatingCubes: Куб {currentCube.color} (offset={currentCube.offset:F3}) слишком далеко от {lowerCube.color} (offset={lowerCube.offset:F3}). Distance={offsetDifference:F3} > Max={maxOffsetDifference:F3}");
                
                // Куб не опирается на нижний - отмечаем к удалению его и все выше
                for (int j = i; j < _cubes.Count; j++)
                {
                    cubesToRemove.Add(j);
                }
                break; // Прерываем цикл, так как все остальные кубы тоже будут удалены
            }
        }
        
        if (cubesToRemove.Count > 0)
        {
            _loggerService?.LogFloatingCubesRemoved(cubesToRemove.Count);
        }
        
        // Удаляем кубы в обратном порядке (чтобы индексы не сбились)
        for (int i = cubesToRemove.Count - 1; i >= 0; i--)
        {
            var cubeToRemove = _cubes[cubesToRemove[i]];
            Debug.Log($"RemoveFloatingCubes: Удаляем висящий куб Color={cubeToRemove.color}, Index={cubesToRemove[i]}");
            _cubes.RemoveAt(cubesToRemove[i]);
        }
        
        // Пересчитываем порядковые номера после удаления висящих кубов
        for (int i = 0; i < _cubes.Count; i++)
        {
            var cube = _cubes[i];
            cube.order = i;
            _cubes[i] = cube;
        }
       
    }
    /// <summary>
    /// Очищает всю башню
    /// </summary>
    public void Clear()
    {
        _cubes.Clear();
        Debug.Log("Tower cleared");
        OnListChanged?.Invoke(_cubes);
    }
}