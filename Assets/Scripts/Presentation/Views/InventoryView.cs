using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// View for inventory (container for inventory cubes)
/// </summary>
public class InventoryView : MonoBehaviour
{
    #region Fields and Dependencies
    [SerializeField] private Transform _inventoryContainer;
    [SerializeField] private ScrollRect _scrollRect;
    private IInventoryConfigService _inventoryConfigService;
    private ICubeFactory _cubeFactory;
    #endregion

    
    [Inject]
    private void Constructor(IInventoryConfigService inventoryConfigService, ICubeFactory cubeFactory)
    {
        _inventoryConfigService = inventoryConfigService;
        _cubeFactory = cubeFactory;
        _inventoryConfigService.OnConfigUpdated += OnInventoryConfigUpdated;
    }
    
    private void CreateCubes()
    {
        // Clear existing cubes
        foreach (Transform child in _inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new cubes based on configuration
        foreach (var color in _inventoryConfigService.Config.availableCubeColors)
        {
            _cubeFactory.CreateInventoryCube(color, _scrollRect, _inventoryContainer);
        }
    }

    private void OnInventoryConfigUpdated()
    {
        // Recreate cubes when configuration updates
        CreateCubes();
    }

    private void OnDestroy()
    {
        if (_inventoryConfigService != null)
        {
            _inventoryConfigService.OnConfigUpdated -= OnInventoryConfigUpdated;
        }
    }
     
}
