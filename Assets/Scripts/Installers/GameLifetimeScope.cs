
using UnityEngine;
using VContainer;
using VContainer.Unity;
public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private RectTransform _towerArea;
    [SerializeField] private RectTransform _holeArea;
    [SerializeField] private RectTransform _holeParent;
    [SerializeField] private RectTransform _effectsArea;
    [SerializeField] private GameObject _inventoryCubePrefab;
    [SerializeField] private GameObject _towerCubePrefab;
    [SerializeField] private CubeEffect _cubeEffectPrefab;

    
    protected override void Configure(IContainerBuilder builder)
    {
       // Services
      
       builder.Register<ILoggerService, LoggerService>(Lifetime.Singleton);
       builder.Register<AutoSaveService>(Lifetime.Singleton);
       builder.Register<TowerService>(Lifetime.Scoped)
           .WithParameter("cubesContainer", _towerArea);
       builder.Register<DragService>(Lifetime.Scoped)
           .WithParameter("towerArea", _towerArea)
           .WithParameter("holeArea", _holeArea)
           .WithParameter("holeParent", _holeParent);
       
       // Factories
       builder.Register<ICubeFactory, CubeFactory>(Lifetime.Scoped)
           .WithParameter("inventoryCubePrefab", _inventoryCubePrefab)
           .WithParameter("towerCubePrefab", _towerCubePrefab);

       builder.Register<EffectsService>(Lifetime.Singleton)
           .WithParameter("cubeEffectPrefab", _cubeEffectPrefab)
           .WithParameter("effectsArea", _effectsArea);

       
       // Presentation
       builder.RegisterComponentInHierarchy<InventoryView>();
       builder.RegisterComponentInHierarchy<GhostCubeView>();
       builder.RegisterComponentInHierarchy<TowerView>();
       builder.RegisterComponentInHierarchy<LoggerView>();
       builder.RegisterComponentInHierarchy<ApplicationLifecycleNotifier>();

    }
}
