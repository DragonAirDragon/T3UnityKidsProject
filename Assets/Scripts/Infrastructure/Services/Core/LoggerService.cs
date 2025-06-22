using System;
using UnityEngine;

/// <summary>
/// Service responsible for logging game events and managing log messages
/// </summary>
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
    
    public void LogCubePlaced(CubeColor color, int index, float offset)
    {
        LogWithFormat("cube_placed", color, index + 1, offset);
    }

    public void LogCubeRemoved(CubeColor color, int index)
    {
        LogWithFormat("cube_removed", color, index + 1);
    }

    public void LogCubeIncorrectHeight(CubeColor color)
    {
        LogWithFormat("cube_incorrect_height", color);
    }

    public void LogCubeIncorrectOffset(CubeColor color)
    {
        LogWithFormat("cube_incorrect_offset", color);
    }

    public void LogCubeIncorrectZone(CubeColor color)
    {
        LogWithFormat("cube_incorrect_zone", color);
    }
    
    public void LogFloatingCubesRemoved(int count)
    {
        LogWithFormat("floating_cubes_removed", count);
    }

    public void LogSaveTower()
    {
        // Direct text keys that don't require formatting
        LogMessage(_localizationService.GetText("tower_saved"));
    }

    public void LogLoadTower()
    {
        // Direct text keys that don't require formatting
        LogMessage(_localizationService.GetText("tower_loaded"));
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

    /// <summary>
    /// Helper method to format and log messages that require parameters
    /// </summary>
    /// <param name="key">Localization key for the message template</param>
    /// <param name="args">Arguments to format into the message template</param>
    private void LogWithFormat(string key, params object[] args)
    {
        string messageTemplate = _localizationService.GetText(key);
        string formattedMessage = string.Format(messageTemplate, args);
        LogMessage(formattedMessage);
    }

    /// <summary>
    /// Core logging method that handles timestamp addition and notification
    /// </summary>
    /// <param name="message">The message to log</param>
    private void LogMessage(string message)
    {
        string timeStamp = DateTime.Now.ToString("HH:mm:ss");
        string logEntry = $"[{timeStamp}] {message}";
        
        _currentLog = logEntry;
        
        OnLogUpdated?.Invoke(_currentLog);
        Debug.Log(message);
    }

    /// <summary>
    /// Handler for language change events to ensure log messages stay up to date
    /// </summary>
    private void OnLanguageChanged()
    {
        // Update current log if it exists when language changes
        if (!string.IsNullOrEmpty(_currentLog))
        {
            OnLogUpdated?.Invoke(_currentLog);
        }
    }
} 