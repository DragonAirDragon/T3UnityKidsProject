using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using DG.Tweening;
using System;

/// <summary>
/// View for tower cube (cube that is in tower)
/// </summary>
public class TowerCubeView : MonoBehaviour, IDraggableCube,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields and Dependencies
    [SerializeField] private Transform _visualCube;
    [SerializeField] private RectTransform _visualCubeTransform;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _parentImage;
    [SerializeField] private Image _childImage;
    public RectTransform RectTransform => _rectTransform;
    private CubeColor _color;
    private int _cubeIndex;
    private bool _isAnimating = false;
    private bool _componentsInitialized = false;
    private GameConfig _gameConfig;
    private IDragService _dragService;
    
    private IEffectsService _effectsService;
    private ICubeDataContainer _cubeDataContainer;
    #endregion

    [Inject]
    public void Construct(IDragService dragService, GameConfig gameConfig, IEffectsService effectsService, ICubeDataContainer cubeDataContainer)
    {
        _dragService = dragService;
        _gameConfig = gameConfig;
        _effectsService = effectsService;
        _cubeDataContainer = cubeDataContainer;
    }
    #region Lifecycle

    private void Awake()
    {
        InitializeComponents();
    }
    private void InitializeComponents()
    {
        if (_componentsInitialized) return;
        _parentImage = GetComponent<Image>();
        if (_visualCube == null && transform.childCount > 0)
        {
            _visualCube = transform.GetChild(0);
        }
        
        if (_visualCube != null)
        {
            _visualCubeTransform = _visualCube.GetComponent<RectTransform>();
            _childImage = _visualCube.GetComponent<Image>();
            if (_childImage != null)
            {
                _childImage.enabled = false;
            }
            _visualCube.gameObject.SetActive(false);
        }

        _componentsInitialized = true;
    }
    private void OnDestroy()
    {
        if (_visualCubeTransform != null)
        {
            _visualCubeTransform.DOKill();
        }
    }
    #endregion
    #region VisualAndAnimations
    public void Setup(CubeColor color)
    {
        _color = color;
        if (_parentImage != null && _gameConfig != null)
        {
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        var sprite = _gameConfig.GetSpriteForCubeColor(_color);
        _parentImage.sprite = sprite;
        if (_childImage != null)
        {
            _childImage.sprite = sprite;
        }
    }
    private void RestoreParentImage()
    {
        if (_parentImage != null)
        {
            _parentImage.enabled = true;
            _parentImage.raycastTarget = true;
        }
        
        if (_childImage != null)
        {
            _childImage.enabled = false;
        }
        
        if (_visualCube != null)
        {
            _visualCube.gameObject.SetActive(false);
        }
    }

    private void SwitchToChildImage()
    {
        if (_parentImage.sprite != _childImage.sprite)
        {
            _childImage.sprite = _parentImage.sprite;
        }
        
        if (_parentImage.color != _childImage.color)
        {
            _childImage.color = _parentImage.color;
        }
        
        _parentImage.enabled = false;
        _parentImage.raycastTarget = false;
        _childImage.enabled = true;
        _visualCube.gameObject.SetActive(true);
    }




    public void SetupGravityAnimation(float yOffset, float cubeHeight)
    {
        if (yOffset <= 0f || !IsReadyForAnimation()) return;
        _isAnimating = true;
        
        SwitchToChildImage();
        Vector3 startPos = Vector3.up * (yOffset * cubeHeight);
        _visualCubeTransform.localPosition = startPos;
        _cubeDataContainer?.ClearGravityEffect(_cubeIndex);
        AnimateGravityFall();
    }



    private bool IsReadyForAnimation()
    {
        return _visualCube != null && _childImage != null && _parentImage != null;
    }


    private void AnimateGravityFall()
    {
        _visualCubeTransform.DOLocalMove(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(OnGravityAnimationComplete);
    }

    private void OnGravityAnimationComplete()
    {
        _isAnimating = false;
        RestoreParentImage();
       
    }
    public void ForceStopAnimation()
    {
        if (_isAnimating && _visualCubeTransform != null)
        {
            _visualCubeTransform.DOKill();
            OnGravityAnimationComplete();
        }
    }
    #endregion
    #region Drag and Drop
    public void SetCubeIndex(int index)
    {
        _cubeIndex = index;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {   
        _dragService?.SetGhostCubeActive(true);
        _dragService?.SetGhostCubeSprite(_color);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragService?.MoveGhost(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragService?.SetGhostCubeActive(false);
        _dragService?.DropCubeToHole(eventData, _color, _cubeIndex);
    }
    
    #endregion
    public void PlayDestructionEffect()
    {
        if (transform is RectTransform rectTransform)
        {
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, rectTransform.position);
            EffectDirection randomDirection = UnityEngine.Random.Range(0, 2) == 0 ? EffectDirection.DownLeft : EffectDirection.DownRight;
            _effectsService?.PlayCubeWrongPlacementEffect(_color, screenPosition, randomDirection);
        }
    }
} 