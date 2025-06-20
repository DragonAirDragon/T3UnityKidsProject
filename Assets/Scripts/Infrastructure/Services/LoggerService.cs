using System;
using UnityEngine;

public class LoggerService : ILoggerService
{
    private string _currentLog = "";
    private readonly ILocalizationService _localizationService;
    public event Action<string> OnLogUpdated;

    public LoggerService(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        _localizationService.OnLanguageChanged += OnLanguageChanged;
    }
    
    public void LogCubePlaced(CubeColor color, int order, float offset)
    {
        string message = string.Format(_localizationService.GetText("cube_placed"), color, order + 1, offset);
        SetLog(message);
    }

    public void LogCubeRemoved(CubeColor color, int order)
    {
        string message = string.Format(_localizationService.GetText("cube_removed"), color, order + 1);
        SetLog(message);
    }

    public void LogCubeIncorrectHeight(CubeColor color)
    {
        string message = string.Format(_localizationService.GetText("cube_incorrect_height"), color);
        SetLog(message);
    }

    public void LogCubeIncorrectOffset(CubeColor color)
    {
        string message = string.Format(_localizationService.GetText("cube_incorrect_offset"), color);
        SetLog(message);
    }

    public void LogCubeIncorrectZone(CubeColor color)
    {
        string message = string.Format(_localizationService.GetText("cube_incorrect_zone"), color);
        SetLog(message);
    }
    
    public void LogFloatingCubesRemoved(int count)
    {
        string message = string.Format(_localizationService.GetText("floating_cubes_removed"), count);
        SetLog(message);
    }

    public void LogSaveTower()
    {
        string message = _localizationService.GetText("tower_saved");
        SetLog(message);
    }

    public void LogLoadTower()
    {
        string message = _localizationService.GetText("tower_loaded");
        SetLog(message);
    }

    public void ClearLog()
    {
        _currentLog = "";
        OnLogUpdated?.Invoke(_currentLog);
    }

    public string GetCurrentLog()
    {
        return _currentLog;
    }

    private void SetLog(string message)
    {
        string timeStamp = DateTime.Now.ToString("HH:mm:ss");
        string logEntry = $"[{timeStamp}] {message}";
        
        _currentLog = logEntry;
        
        OnLogUpdated?.Invoke(_currentLog);
        Debug.Log(logEntry);
    }

    private void OnLanguageChanged()
    {
        // При смене языка обновляем текущий лог, если он есть
        if (!string.IsNullOrEmpty(_currentLog))
        {
            OnLogUpdated?.Invoke(_currentLog);
        }
    }


} 