using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Service for saving tower data to file and loading it
/// </summary>
public sealed class AutoSaveService
{
    private readonly TowerService _towerService;
    private readonly ISaveService _saveService;
    private readonly ILoggerService _loggerService;
    private readonly ICubeDataContainer _cubeDataContainer;

    public AutoSaveService(TowerService towerService, ISaveService saveService, ILoggerService loggerService, ICubeDataContainer cubeDataContainer)
    {
        _towerService = towerService;
        _saveService = saveService;
        _loggerService = loggerService;
        _cubeDataContainer = cubeDataContainer;
    }
    
    public void SaveGame()
    {
        var currentCubes = new List<CubeData>(_cubeDataContainer.Cubes);
        _saveService.SaveTowerData(currentCubes);
        _loggerService?.LogSaveTower();
    }
    
    public void LoadGame()
    {
        var savedCubes = _saveService.LoadTowerData();
        _cubeDataContainer.LoadData(savedCubes);
        _loggerService?.LogLoadTower();
    }


    public void ClearSave()
    {
        _saveService.ClearSavedData();
        _cubeDataContainer.Clear();
    }
} 