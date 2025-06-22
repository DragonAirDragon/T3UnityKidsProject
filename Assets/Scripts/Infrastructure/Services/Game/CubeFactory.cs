using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// Service responsible for creating cubes
/// </summary>
public class CubeFactory : ICubeFactory
{
    private readonly IObjectResolver _container;
    private readonly GameConfig _gameConfig;
    private readonly GameObject _inventoryCubePrefab;
    private readonly GameObject _towerCubePrefab;
    

    public CubeFactory(IObjectResolver container, GameConfig gameConfig, GameObject inventoryCubePrefab, GameObject towerCubePrefab)
    {
        _container = container;
        _gameConfig = gameConfig;
        _inventoryCubePrefab = inventoryCubePrefab;
        _towerCubePrefab = towerCubePrefab;
    }

    public GameObject CreateInventoryCube(CubeColor color, ScrollRect scrollRect, Transform parent)
    {
        GameObject cubeObject = Object.Instantiate(_inventoryCubePrefab, parent);
        
        Image cubeImage = cubeObject.GetComponent<Image>();
        if (cubeImage != null)
        {
            cubeImage.sprite = _gameConfig.GetSpriteForCubeColor(color);
        }
        
        InventoryCubeView inventoryView = cubeObject.GetComponent<InventoryCubeView>();
        if (inventoryView != null)
        {
            _container.Inject(inventoryView);
            inventoryView.Setup(color);
            inventoryView.SetScrollRect(scrollRect);
        }
        
        return cubeObject;
    }

    public GameObject CreateTowerCube(CubeData cubeData, Transform parent)
    {
        // Create cube
        GameObject cubeObject = Object.Instantiate(_towerCubePrefab, parent);
        
        // Configure size (sprite will be set in TowerCubeView.Setup())
        RectTransform rectTransform = cubeObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(_gameConfig.cubeWidth, _gameConfig.cubeHeight);
        }
        
        // Get component and inject dependencies through container
        TowerCubeView towerView = cubeObject.GetComponent<TowerCubeView>();
        if (towerView != null)
        {
            _container.Inject(towerView);
            // Setup with color will be called later in TowerView
        }
        
        return cubeObject;
    }
    
} 