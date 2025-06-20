using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TowerView : MonoBehaviour
{
    [SerializeField] private Transform _cubesContainer;
    
    private TowerService _towerService;
    private ICubeFactory _cubeFactory;
    private List<GameObject> _spawnedCubes = new();

    [Inject]
    public void Construct(TowerService towerService, ICubeFactory cubeFactory)
    {
        _towerService = towerService;
        _cubeFactory = cubeFactory;
    }

    private void Start()
    {
        
        // Подписываемся на события изменения башни
        _towerService.OnListChanged += OnTowerChanged;
        Debug.Log("TowerView: Подписался на события TowerService");
        
        // Отображаем текущее состояние башни
        OnTowerChanged(_towerService.Cubes as List<CubeData>);
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (_towerService != null)
        {
            _towerService.OnListChanged -= OnTowerChanged;
        }
    }

    private void OnTowerChanged(List<CubeData> cubes)
    {
        Debug.Log($"TowerView: Обновляю визуализацию башни. Количество кубов: {cubes.Count}");
        
        // Удаляем все текущие кубы
        ClearSpawnedCubes();
        
        // Создаем новые кубы на основе данных
        for (int i = 0; i < cubes.Count; i++)
        {
            CubeData? lastCube = i > 0 ? cubes[i - 1] : null;
            SpawnCube(cubes[i], lastCube);
        }
    }

    private void SpawnCube(CubeData cubeData, CubeData? lastCubeData)
    {
        // Создаем новый куб через фабрику
        GameObject cubeObject = _cubeFactory.CreateTowerCube(cubeData, _cubesContainer);
        _spawnedCubes.Add(cubeObject);
        
        // Получаем позицию от сервиса и применяем её
        RectTransform cubesContainerRect = _cubesContainer.GetComponent<RectTransform>();
        Vector2 position = _towerService.GetCubePosition(cubeData, cubesContainerRect);
        
        RectTransform rectTransform = cubeObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = position;
        }
        
        //Debug.Log($"TowerView: Создан куб {cubeData.color} в позиции {position} (Order: {cubeData.order}, Offset: {cubeData.offset:F3})");
    }
    private void ClearSpawnedCubes()
    {
        // Удаляем все созданные кубы
        foreach (var cube in _spawnedCubes)
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }
        _spawnedCubes.Clear();
    }
    // Методы для тестирования в редакторе
    [ContextMenu("Force Refresh Tower")]
    private void ForceRefreshTower()
    {
        if (_towerService != null)
        {
            OnTowerChanged(_towerService.Cubes as List<CubeData>);
        }
    }
} 