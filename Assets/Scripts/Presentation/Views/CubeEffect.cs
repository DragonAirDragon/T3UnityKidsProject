using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Effect cube unit
/// </summary>
public class CubeEffect : MonoBehaviour
{
    #region Fields and Dependencies
    [SerializeField] private Image _cubeImage;
    [SerializeField] private RectTransform _rectTransform;
    #endregion

    #region Lifecycle
    private void OnDestroy()
    {
        transform.DOKill();
        _cubeImage?.DOKill();
        _rectTransform?.DOKill();
    }
    public void Setup(Sprite cubeSprite, Vector2 position)
    {
        _cubeImage.sprite = cubeSprite;
        _rectTransform.anchoredPosition = position;
        
        // Reset transparency and scale
        var color = _cubeImage.color;
        color.a = 1f;
        _cubeImage.color = color;
        transform.localScale = Vector3.one;
    }
    #endregion

    #region Effects
    public void PlayDissolveEffect(float animationDuration)
    {
        var sequence = DOTween.Sequence();
        
        sequence.Append(_cubeImage.DOFade(0f, animationDuration).SetEase(Ease.InQuart));
        
        // Slight size reduction for dissolve effect
        sequence.Join(transform.DOScale(0.8f, animationDuration).SetEase(Ease.InQuart));
        
        sequence.OnComplete(() => {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
        
        sequence.SetTarget(transform);
    }
    public void PlayDissolveEffectWithFall(EffectDirection direction, float animationDuration, float fallDistance)
    {
        var sequence = DOTween.Sequence();
        
        Vector2 fallDirection = GetFallDirection(direction);
        Vector2 targetPosition = _rectTransform.anchoredPosition + fallDirection * fallDistance;
        
        sequence.Append(_rectTransform.DOAnchorPos(targetPosition, animationDuration).SetEase(Ease.InQuart));
        
        // Fade animation starts at 30% of the time
        sequence.Insert(animationDuration * 0.3f, _cubeImage.DOFade(0f, animationDuration * 0.7f).SetEase(Ease.InQuart));
        
        
        float rotation = 0f;
        
        if(direction == EffectDirection.DownRight || direction == EffectDirection.Right){
            rotation = Random.Range(-180f, -100f);
        }
        
        else if(direction == EffectDirection.DownLeft || direction == EffectDirection.Left){
            rotation = Random.Range(100f, 180f);
        }

        // Small rotation for realistic falling
        sequence.Join(transform.DORotate(new Vector3(0, 0, rotation), animationDuration).SetEase(Ease.InQuart));
        
        sequence.OnComplete(() => {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
        
        sequence.SetTarget(transform);
    }
    
    public void PlayFallEffect(EffectDirection direction, float fallDistance, float animationDuration)
    {
        var sequence = DOTween.Sequence();
        Vector2 fallDirection = GetFallDirection(direction);
        Vector2 targetPosition = _rectTransform.anchoredPosition + fallDirection * fallDistance;
        
        sequence.Append(_rectTransform.DOAnchorPos(targetPosition, animationDuration).SetEase(Ease.InCubic));
        sequence.Join(transform.DORotate(new Vector3(0, 0, Random.Range(-180f, 180f)), animationDuration).SetEase(Ease.InCubic));
        // Fade out only at the very end
        sequence.Append(_cubeImage.DOFade(0f, 0.2f));
        
        sequence.OnComplete(() => {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
        
        sequence.SetTarget(transform);
    }
    
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
    #endregion
}
