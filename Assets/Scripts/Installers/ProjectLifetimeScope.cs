using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
/// <summary>
/// Lifetime scope in the project
/// </summary>
public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private InventoryConfig _inventoryConfig;
    [SerializeField] private AnimationsConfig _animationsConfig;
    [SerializeField] private LocalizationConfig _localizationConfig;
    
    protected override void Configure(IContainerBuilder builder)
    {
        // Game Config
        builder.RegisterInstance(_gameConfig);
        
        // Inventory Config

        var inventorySources = new List<IInventoryConfigSource>
        {
            // Example not working option
            new RemoteInventoryConfigSource("https://example-server.com/configs/inventory.json",
                timeoutSeconds: 1f),
            new ScriptableObjectInventoryConfigSource(_inventoryConfig)
        };

        builder.RegisterInstance<IEnumerable<IInventoryConfigSource>>(inventorySources);

        builder.Register<InventoryConfigService>(Lifetime.Singleton)
            .As<IInventoryConfigService>();


        // Animations Config

        var animationsSources = new List<IAnimationsConfigSource>
        {
            new ScriptableObjectAnimationsConfigSource(_animationsConfig)
        };

        builder.RegisterInstance<IEnumerable<IAnimationsConfigSource>>(animationsSources);

        builder.Register<AnimationsConfigService>(Lifetime.Singleton)
            .As<IAnimationsConfigService>();
       
    
       
        // Localization Config
        var localizationSources = new List<ILocalizationConfigSource>
        {
            new ScriptableObjectLocalizationConfigSource(_localizationConfig)
        };

        builder.RegisterInstance<IEnumerable<ILocalizationConfigSource>>(localizationSources);

        builder.Register<LocalizationConfigService>(Lifetime.Singleton)
            .As<ILocalizationConfigService>();
       
       
       // Services
       builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
       builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
    }
}
