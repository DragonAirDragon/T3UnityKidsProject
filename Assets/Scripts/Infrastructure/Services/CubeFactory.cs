using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

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
        // Создаем куб
        GameObject cubeObject = Object.Instantiate(_inventoryCubePrefab, parent);
        
        // Настраиваем спрайт
        Image cubeImage = cubeObject.GetComponent<Image>();
        if (cubeImage != null)
        {
            cubeImage.sprite = _gameConfig.GetSpriteForCubeColor(color);
        }
        
        // Получаем компонент и инжектим зависимости через контейнер
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
        // Создаем куб
        GameObject cubeObject = Object.Instantiate(_towerCubePrefab, parent);
        
        // Настраиваем спрайт
        Image cubeImage = cubeObject.GetComponent<Image>();
        if (cubeImage != null)
        {
            cubeImage.sprite = _gameConfig.GetSpriteForCubeColor(cubeData.color);
        }
        
        // Настраиваем позицию и размер
        RectTransform rectTransform = cubeObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector2 size = new Vector2(_gameConfig.cubeWidth, _gameConfig.cubeHeight);
            rectTransform.sizeDelta = size;
        }
        
        // Получаем компонент и инжектим зависимости через контейнер
        TowerCubeView towerView = cubeObject.GetComponent<TowerCubeView>();
        if (towerView != null)
        {
            _container.Inject(towerView);
            towerView.Setup(cubeData.color);
            towerView.SetCubeIndex(cubeData.order);
        }
        
        return cubeObject;
    }
    
} 