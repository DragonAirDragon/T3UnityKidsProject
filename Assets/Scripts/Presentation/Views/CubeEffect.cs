using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CubeEffect : MonoBehaviour
{
    [SerializeField] private Image _cubeImage;
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _fallDistance = 200f;
    [SerializeField] private RectTransform _rectTransform;

    private void OnDestroy()
    {
        // Останавливаем все анимации, связанные с этим объектом
        transform.DOKill();
        _cubeImage?.DOKill();
        _rectTransform?.DOKill();
    }
    
    /// <summary>
    /// Настраивает цвет и позицию эффекта
    /// </summary>
    public void Setup(Sprite cubeSprite, Vector2 position)
    {
        _cubeImage.sprite = cubeSprite;
        _rectTransform.anchoredPosition = position;
        
        // Сбрасываем прозрачность и масштаб
        var color = _cubeImage.color;
        color.a = 1f;
        _cubeImage.color = color;
        transform.localScale = Vector3.one;
    }
    
    /// <summary>
    /// Простая анимация исчезновения (Dissolve)
    /// </summary>
    public void PlayDissolveEffect()
    {
        var sequence = DOTween.Sequence();
        
        // Анимация исчезновения
        sequence.Append(_cubeImage.DOFade(0f, _animationDuration).SetEase(Ease.InQuart));
        
        // Небольшое уменьшение размера для эффекта растворения
        sequence.Join(transform.DOScale(0.8f, _animationDuration).SetEase(Ease.InQuart));
        
        // Уничтожение объекта после анимации
        sequence.OnComplete(() => {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
        
        // Привязываем последовательность к объекту для автоматической очистки
        sequence.SetTarget(transform);
    }
    
    /// <summary>
    /// Анимация исчезновения с падением
    /// </summary>
    public void PlayDissolveEffectWithFall(EffectDirection direction)
    {
        var sequence = DOTween.Sequence();
        
        Vector2 fallDirection = GetFallDirection(direction);
        Vector2 targetPosition = _rectTransform.anchoredPosition + fallDirection * _fallDistance;
        
        // Анимация падения
        sequence.Append(_rectTransform.DOAnchorPos(targetPosition, _animationDuration).SetEase(Ease.InQuart));
        
        // Анимация исчезновения (начинается через 30% времени)
        sequence.Insert(_animationDuration * 0.3f, _cubeImage.DOFade(0f, _animationDuration * 0.7f).SetEase(Ease.InQuart));
        
        
        float rotation = 0f;
        
        if(direction == EffectDirection.DownRight || direction == EffectDirection.Right){
            rotation = Random.Range(-180f, -100f);
        }
        
        else if(direction == EffectDirection.DownLeft || direction == EffectDirection.Left){
            rotation = Random.Range(100f, 180f);
        }

        // Небольшое вращение для реалистичности падения
        sequence.Join(transform.DORotate(new Vector3(0, 0, rotation), _animationDuration).SetEase(Ease.InQuart));
        
        // Уничтожение объекта после анимации
        sequence.OnComplete(() => {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
        
        // Привязываем последовательность к объекту для автоматической очистки
        sequence.SetTarget(transform);
    }
    
    /// <summary>
    /// Анимация только падения без исчезновения
    /// </summary>
    public void PlayFallEffect(EffectDirection direction, float fallDistance, float animationDuration)
    {
        var sequence = DOTween.Sequence();
        Vector2 fallDirection = GetFallDirection(direction);
        Vector2 targetPosition = _rectTransform.anchoredPosition + fallDirection * fallDistance;
        // Анимация падения
        sequence.Append(_rectTransform.DOAnchorPos(targetPosition, _animationDuration).SetEase(Ease.InCubic));
        // Вращение при падении
        sequence.Join(transform.DORotate(new Vector3(0, 0, Random.Range(-180f, 180f)), _animationDuration).SetEase(Ease.InCubic));
        // Исчезновение только в самом конце
        sequence.Append(_cubeImage.DOFade(0f, 0.2f));
        // Уничтожение объекта после анимации
        sequence.OnComplete(() => {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
        
        // Привязываем последовательность к объекту для автоматической очистки
        sequence.SetTarget(transform);
    }
    
    /// <summary>
    /// Конвертирует направление эффекта в вектор направления
    /// </summary>
    private Vector2 GetFallDirection(EffectDirection direction)
    {
        return direction switch
        {
            EffectDirection.Down => Vector2.down,
            EffectDirection.Left => Vector2.left,
            EffectDirection.Right => Vector2.right,
            EffectDirection.DownLeft => new Vector2(-1f, -1f).normalized,
            EffectDirection.DownRight => new Vector2(1f, -1f).normalized,
            _ => Vector2.down
        };
    }
}
