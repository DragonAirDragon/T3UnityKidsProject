using System.Collections.Generic;
using UnityEngine;
using VContainer;

/// <summary>
/// View for tower (container for tower cubes)
/// </summary>
public class TowerView : MonoBehaviour
{
    #region Fields and Dependencies
    
    [SerializeField] private Transform _cubesContainer;
    [SerializeField] private RectTransform _cubesContainerRect;
    private TowerService _towerService;
    private ICubeFactory _cubeFactory;
    private Queue<TowerCubeView> _cubePool = new Queue<TowerCubeView>();
    private List<TowerCubeView> _activeCubes = new List<TowerCubeView>();
    private ICubeDataContainer _cubeDataContainer;
    private const int INITIAL_POOL_SIZE = 20;
    
    #endregion
    
    #region Lifecycle
    
    [Inject]
    public void Construct(TowerService towerService, ICubeFactory cubeFactory, ICubeDataContainer cubeDataContainer)
    {
        _towerService = towerService;
        _cubeFactory = cubeFactory;
        _cubeDataContainer = cubeDataContainer;
    }

    private void Start()
    {
        if (_cubeFactory != null && _towerService != null)
        {
            InitializePool();
            _cubeDataContainer.OnDataChanged += OnTowerChanged;
            _towerService.OnCubesNeedDestructionEffect += OnCubesNeedDestructionEffect;
            OnTowerChanged(_towerService.Cubes as List<CubeData>);
        }
        else
        {
            Debug.LogError("TowerView: _cubeFactory or _towerService not initialized! Check DI container.");
        }
    }


    private void OnDestroy()
    {
        if (_towerService != null)
        {
            _cubeDataContainer.OnDataChanged -= OnTowerChanged;
            _towerService.OnCubesNeedDestructionEffect -= OnCubesNeedDestructionEffect;
        }
    }
    
    #endregion
    
    #region Initialization

    private void InitializePool()
    {
        if (_cubeFactory == null)
        {
            Debug.LogError("TowerView.InitializePool(): _cubeFactory is null!");
            return;
        }
        
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            var cubeData = new CubeData { color = 0, offset = 0f };
            GameObject cube = _cubeFactory.CreateTowerCube(cubeData, _cubesContainer);
            cube.SetActive(false);
            _cubePool.Enqueue(cube.GetComponent<TowerCubeView>());
        }
    }
    
    #endregion
    
    #region Event Handling

    private void OnTowerChanged(List<CubeData> cubes)
    {
        // Simple and reliable logic
        ClearAllCubes();
        
        // Create all cubes with correct indices
        for (int i = 0; i < cubes.Count; i++)
        {
            CreateSingleCube(cubes[i], i);
        }
    }

    private void ClearAllCubes()
    {
        while (_activeCubes.Count > 0)
        {
            ReturnCubeToPool(0);
        }
    }

    private void CreateSingleCube(CubeData cubeData, int cubeIndex)
    {
        TowerCubeView cubeView = GetCubeFromPool();
        _activeCubes.Add(cubeView);
        
        cubeView.RectTransform.anchoredPosition = _towerService.GetCubePosition(cubeData, _cubesContainerRect, cubeIndex);
        cubeView.Setup(cubeData.color);
        cubeView.SetCubeIndex(cubeIndex);
        
        // Animation ONLY for cubes with yOffset > 0 (new or falling cubes)
        if (cubeData.yOffset > 0f)
        {
            Vector2 actualCubeSize = _towerService.GetCubeSize();
            cubeView.SetupGravityAnimation(cubeData.yOffset, actualCubeSize.y);
        }
    }

    /// <summary>
    /// Handles the event for playing destruction effects on cubes
    /// </summary>
    private void OnCubesNeedDestructionEffect(List<int> cubeIndices)
    {
        foreach (int cubeIndex in cubeIndices)
        {
            if (cubeIndex >= 0 && cubeIndex < _activeCubes.Count)
            {
                var cubeObject = _activeCubes[cubeIndex];
                var towerCubeView = cubeObject.GetComponent<TowerCubeView>();
                towerCubeView?.PlayDestructionEffect();
            }
        }
    }
    
    #endregion
    
    #region Cube Management


    
    #endregion
    
    #region Object Pool Management

    private TowerCubeView GetCubeFromPool()
    {
        TowerCubeView cube;
        
        if (_cubePool.Count > 0)
        {
            cube = _cubePool.Dequeue();
        }
        else
        {
            var cubeData = new CubeData { color = 0, offset = 0f };
            GameObject cubeObject = _cubeFactory.CreateTowerCube(cubeData, _cubesContainer);
            cube = cubeObject.GetComponent<TowerCubeView>();
        }
        
        cube.gameObject.SetActive(true);
        return cube;
    }

    private void ReturnCubeToPool(int index)
    {
        if (index >= 0 && index < _activeCubes.Count)
        {
            TowerCubeView cube = _activeCubes[index];
            _activeCubes.RemoveAt(index);
            
            cube.gameObject.SetActive(false);
            _cubePool.Enqueue(cube);
        }
    }
    
    #endregion
    
    #region Debug Methods

    [ContextMenu("Force Refresh Tower")]
    private void ForceRefreshTower()
    {
        if (_towerService != null)
        {
            OnTowerChanged(_towerService.Cubes as List<CubeData>);
        }
    }
    
    #endregion
} 