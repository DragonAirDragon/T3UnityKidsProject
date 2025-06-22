using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Animations configuration service with multiple sources support and fallback
/// </summary>
public class AnimationsConfigService : IAnimationsConfigService
{
    private readonly ILoggerService _logger;
    private AnimationsConfigData _config;

    public AnimationsConfigData Config 
    { 
        get 
        { 
            if (_config == null)
                throw new System.InvalidOperationException("AnimationsConfig not loaded yet!");
            return _config; 
        } 
    }

    public AnimationsConfigService(
        IEnumerable<IAnimationsConfigSource> sources)
    {
        _ = LoadConfigAsync(sources.ToArray());
    }

    private async UniTask LoadConfigAsync(IAnimationsConfigSource[] sources)
    {
        foreach (var source in sources)
        {
            try
            {   
                var (success, data) = await source.LoadAsync();
                
                if (success && data != null)
                {
                    _config = data;
                    Debug.Log($"Animations config loaded successfully from: {source.SourceName}");
                    return;
                }
                
                Debug.LogWarning($"Failed to load from: {source.SourceName}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception loading from {source.SourceName}: {ex.Message}");
            }
        }

        Debug.LogWarning("All animations config sources failed, using default config");
        _config = new AnimationsConfigData();
    }
}