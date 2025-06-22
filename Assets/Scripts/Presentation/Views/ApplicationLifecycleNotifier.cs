using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;

/// <summary>
/// Notifier for application lifecycle events
/// </summary>
public sealed class ApplicationLifecycleNotifier : MonoBehaviour
{
    private AutoSaveService _autoSaveService;

    [Inject]
    public void Construct(AutoSaveService autoSaveService)
    {
        _autoSaveService = autoSaveService;
    }
    
    #region Lifecycle
    private void Start()
    {
        LoadGameDelayed().Forget();
    }

    private async UniTaskVoid LoadGameDelayed()
    {
        await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
        _autoSaveService?.LoadGame();
    }

    private void OnApplicationQuit()
    {
        _autoSaveService?.SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            _autoSaveService?.SaveGame();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            _autoSaveService?.SaveGame();
        }
    }

    private void OnDestroy()
    {
        _autoSaveService?.SaveGame();
    }
    #endregion

    #region Debug
    [ContextMenu("ClearSave")]
    private void ClearSave()
    {
        _autoSaveService?.ClearSave();
    }
    #endregion
} 