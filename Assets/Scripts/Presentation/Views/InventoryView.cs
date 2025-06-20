using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Transform _inventoryContainer;
    [SerializeField] private ScrollRect _scrollRect;
    private GameConfig _gameConfig;
    private ICubeFactory _cubeFactory;
    
    [Inject]
    private void Constructor(GameConfig gameConfig, ICubeFactory cubeFactory)
    {
        _gameConfig = gameConfig;
        _cubeFactory = cubeFactory;
    }
    
    private void Start()
    {
        CreateCubes();
    }
    
    private void CreateCubes()
    {
        foreach (var color in _gameConfig.availableCubeColors)
        {
            _cubeFactory.CreateInventoryCube(color, _scrollRect, _inventoryContainer);
        }
    }
     
}
