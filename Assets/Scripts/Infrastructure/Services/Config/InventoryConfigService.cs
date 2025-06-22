using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Inventory configuration service with multiple sources support and fallback
/// </summary>
public class InventoryConfigService : IInventoryConfigService
{
    private InventoryConfigData _config;
    private bool _isLoaded = false;

    public InventoryConfigData Config 
    { 
        get 
        { 
            if (_config == null)
                return CreateDefaultConfig(); 
            return _config; 
        } 
    }

    public bool IsLoaded => _isLoaded;
    public System.Action OnConfigUpdated { get; set; }

    public InventoryConfigService(
        IEnumerable<IInventoryConfigSource> sources)
    {
        _config = CreateDefaultConfig();
        _isLoaded = true;
        _ = LoadConfigAsync(sources.ToArray());
    }

    private async UniTask LoadConfigAsync(IInventoryConfigSource[] sources)
    {
       

        foreach (var source in sources)
        {
            try
            {   
                var (success, data) = await source.LoadAsync();
                
                if (success && data != null)
                {
                    _config = data;
                    Debug.Log($"✓ Inventory config loaded successfully from: {source.SourceName}");
                    OnConfigUpdated?.Invoke();
                    return;
                }
                
                Debug.LogWarning($"✗ Failed to load inventory config from: {source.SourceName}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"✗ Exception loading inventory config from {source.SourceName}: {ex.Message}");
            }
        }

        Debug.LogWarning("All inventory config sources failed, using default config");
    }

    private InventoryConfigData CreateDefaultConfig()
    {
        var defaultConfig = new InventoryConfigData
        {
            availableCubeColors = new CubeColor[] 
            { 
                CubeColor.Red, 
                CubeColor.BrightCyan, 
                CubeColor.Green, 
                CubeColor.Yellow 
            }
        };

        return defaultConfig;
    }
}