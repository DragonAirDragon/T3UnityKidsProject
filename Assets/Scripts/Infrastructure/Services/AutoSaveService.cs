using UnityEngine;
using System.Collections.Generic;

public class AutoSaveService
{
    private readonly TowerService _towerService;
    private readonly ISaveService _saveService;
    private readonly ILoggerService _loggerService;

    public AutoSaveService(TowerService towerService, ISaveService saveService, ILoggerService loggerService)
    {
        _towerService = towerService;
        _saveService = saveService;
        _loggerService = loggerService;
    }
    
    public void SaveGame()
    {
        var currentCubes = new List<CubeData>(_towerService.Cubes);
        _saveService.SaveTowerData(currentCubes);
        _loggerService?.LogSaveTower();
    }
    
    public void LoadGame()
    {
        var savedCubes = _saveService.LoadTowerData();
        _towerService.LoadTowerData(savedCubes);
        _loggerService?.LogLoadTower();
    }


    public void ClearSave()
    {
        _saveService.ClearSavedData();
        _towerService.Clear();
    }
} 