using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Localization configuration service with multiple sources support and fallback
/// </summary>
public class LocalizationConfigService : ILocalizationConfigService
{
    private LocalizationConfigData _config;

    public LocalizationConfigData Config 
    { 
        get 
        { 
            if (_config == null)
                throw new System.InvalidOperationException("LocalizationConfig not loaded yet!");
            return _config; 
        } 
    }

    public System.Action OnConfigUpdated { get; set; }

    public LocalizationConfigService(
        IEnumerable<ILocalizationConfigSource> sources 
    )
    {
        _ = LoadConfigAsync(sources.ToArray());
    }

    private async UniTask LoadConfigAsync(ILocalizationConfigSource[] sources)
    {
        foreach (var source in sources)
        {
            try
            {
                var (success, data) = await source.LoadAsync();
                
                if (success && data != null)
                {
                    _config = data;
                    Debug.Log($"✓ Localization config loaded successfully from: {source.SourceName}");
                    OnConfigUpdated?.Invoke();
                    return;
                }
                
                Debug.LogWarning($"✗ Failed to load localization from: {source.SourceName}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"✗ Exception loading localization from {source.SourceName}: {ex.Message}");
            }
        }

        Debug.LogWarning("All localization config sources failed, using minimal default config");
        _config = CreateDefaultConfig();
        OnConfigUpdated?.Invoke();
    }

    private LocalizationConfigData CreateDefaultConfig()
    {
        var defaultConfig = new LocalizationConfigData();
        
        var russianTranslations = new Dictionary<string, string>
        {
            { "logs_placeholder", "Логи действий игрока" },
            { "cube_placed", "Размещен куб {0} на позиции {1} со смещением {2:F2}" },
            { "cube_removed", "Удален куб {0} с позиции {1}" },
            { "cube_incorrect_height", "Куб {0} не может быть размещен: превышена максимальная высота" },
            { "cube_incorrect_offset", "Куб {0} не может быть размещен: слишком большое смещение" },
            { "cube_incorrect_zone", "Куб {0} упал в неправильную зону" },
            { "floating_cubes_removed", "Удалено {0} висящих кубов" },
            { "tower_saved", "Башня сохранена" },
            { "tower_loaded", "Башня загружена" }
        };
        defaultConfig.SetTranslationsForLanguage(Language.Russian, russianTranslations);

        var englishTranslations = new Dictionary<string, string>
        {
            { "logs_placeholder", "Player action logs" },
            { "cube_placed", "Placed cube {0} at position {1} with offset {2:F2}" },
            { "cube_removed", "Removed cube {0} from position {1}" },
            { "cube_incorrect_height", "Cube {0} cannot be placed: maximum height exceeded" },
            { "cube_incorrect_offset", "Cube {0} cannot be placed: offset too large" },
            { "cube_incorrect_zone", "Cube {0} fell into incorrect zone" },
            { "floating_cubes_removed", "Removed {0} floating cubes" },
            { "tower_saved", "Tower saved" },
            { "tower_loaded", "Tower loaded" }
        };
        defaultConfig.SetTranslationsForLanguage(Language.English, englishTranslations);

        return defaultConfig;
    }
}