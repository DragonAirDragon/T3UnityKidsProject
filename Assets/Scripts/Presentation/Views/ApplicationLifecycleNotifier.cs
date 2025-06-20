using UnityEngine;
using System.Collections;
using VContainer;

public class ApplicationLifecycleNotifier : MonoBehaviour
{
    private AutoSaveService _autoSaveService;

    [Inject]
    public void Construct(AutoSaveService autoSaveService)
    {
        _autoSaveService = autoSaveService;
    }

    private void Start()
    {
        // Загружаем с задержкой на один кадр для инициализации UI
        StartCoroutine(LoadGameDelayed());
    }

    private IEnumerator LoadGameDelayed()
    {
        yield return null; // Ждём один кадр
        _autoSaveService?.LoadGame();
    }

    private void OnApplicationQuit()
    {
        // Это срабатывает при остановке в редакторе
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

    [ContextMenu("ClearSave")]
    private void ClearSave()
    {
        _autoSaveService?.ClearSave();
    }
} 