using UnityEngine;
using TMPro;
using VContainer;
using DG.Tweening;

/// <summary>
/// View for logger (logs container)
/// </summary>
public class LoggerView : MonoBehaviour
{
    #region Fields and Dependencies
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private float _animationDuration = 0.5f;
    
    private ILoggerService _loggerService;
    private ILocalizationService _localizationService;
    #endregion

    [Inject]
    private void Constructor(ILoggerService loggerService, ILocalizationService localizationService)
    {
        _loggerService = loggerService;
        _localizationService = localizationService;
    }
    #region Lifecycle
    private void Start()
    {
        if (_loggerService != null)
        {
            _loggerService.OnLogUpdated += UpdateLogDisplay;
        }
        
        if (_localizationService != null)
        {
            _localizationService.OnLanguageChanged += OnLanguageChanged;
        }
        
        ShowPlaceholder();
    }
    
    private void OnDestroy()
    {
        _logText?.DOKill();
        
        if (_loggerService != null)
        {
            _loggerService.OnLogUpdated -= UpdateLogDisplay;
        }
        
        if (_localizationService != null)
        {
            _localizationService.OnLanguageChanged -= OnLanguageChanged;
        }
    }
    #endregion

    #region UI
    private void ShowPlaceholder()
    {
        if (_logText == null) return;
        
        string placeholderText = _localizationService?.GetText("logs_placeholder") ?? "Logs will be displayed here...";
        _logText.text = placeholderText;
        _logText.alpha = 0.5f; // Placeholder semi-transparent
    }
    
    private void UpdateLogDisplay(string log)
    {
        if (_logText == null) return;
        
        _logText.DOKill();
        
        if (string.IsNullOrEmpty(log))
        {
            ShowPlaceholder();
            return;
        }
        
        ShowNewLog(log);
    }
    
    private void ShowNewLog(string log)
    {
        _logText.text = log;
        _logText.alpha = 0f;
        
        _logText.DOFade(1f, _animationDuration)
            .SetEase(Ease.OutQuad)
            .SetTarget(transform);
    }

    private void OnLanguageChanged()
    {
        // Update display when language changes
        string currentLog = _loggerService?.GetCurrentLog();
        if (string.IsNullOrEmpty(currentLog))
        {
            ShowPlaceholder();
        }
    }
    #endregion
} 