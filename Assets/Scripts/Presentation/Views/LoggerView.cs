using UnityEngine;
using TMPro;
using VContainer;
using DG.Tweening;

public class LoggerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;
    [SerializeField] private float _animationDuration = 0.5f;
    
    private ILoggerService _loggerService;
    private ILocalizationService _localizationService;
    
    [Inject]
    private void Constructor(ILoggerService loggerService, ILocalizationService localizationService)
    {
        _loggerService = loggerService;
        _localizationService = localizationService;
    }
    
    private void Start()
    {
        _loggerService.OnLogUpdated += UpdateLogDisplay;
        _localizationService.OnLanguageChanged += OnLanguageChanged;
        
        // Показываем placeholder
        ShowPlaceholder();
    }
    
    private void OnDestroy()
    {
        // Останавливаем анимации
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
    
    private void ShowPlaceholder()
    {
        if (_logText == null) return;
        
        _logText.text = _localizationService.GetText("logs_placeholder");
        _logText.alpha = 0.5f; // Placeholder полупрозрачный
    }
    
    private void UpdateLogDisplay(string log)
    {
        if (_logText == null) return;
        
        // Останавливаем предыдущую анимацию
        _logText.DOKill();
        
        if (string.IsNullOrEmpty(log))
        {
            // Возвращаемся к placeholder
            ShowPlaceholder();
            return;
        }
        
        // Показываем новый лог с анимацией
        ShowNewLog(log);
    }
    
    private void ShowNewLog(string log)
    {
        // Устанавливаем текст и делаем его невидимым
        _logText.text = log;
        _logText.alpha = 0f;
        
        // Плавно появляется
        _logText.DOFade(1f, _animationDuration)
            .SetEase(Ease.OutQuad)
            .SetTarget(transform);
    }

    private void OnLanguageChanged()
    {
        // При смене языка обновляем отображение
        string currentLog = _loggerService?.GetCurrentLog();
        if (string.IsNullOrEmpty(currentLog))
        {
            ShowPlaceholder();
        }
    }
} 