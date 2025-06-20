using UnityEngine;
using VContainer;
using VContainer.Unity;
public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private LocalizationConfig _localizationConfig;
    protected override void Configure(IContainerBuilder builder)
    {
       // Configs
       builder.RegisterInstance(_gameConfig);
       builder.RegisterInstance(_localizationConfig);
       // Services
       builder.Register<ILocalizationService, LocalizationService>(Lifetime.Singleton);
       builder.Register<ISaveService, SaveService>(Lifetime.Singleton);
    }
}
