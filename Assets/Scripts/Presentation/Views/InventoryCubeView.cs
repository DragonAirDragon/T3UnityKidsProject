using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// View for inventory cube (cube that is in inventory)
/// </summary>
public class InventoryCubeView : MonoBehaviour, IDraggableCube,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields and Dependencies
    [SerializeField] private CubeColor _color;
    private IDragService _dragService;
    private ScrollRect _parentScrollRect;
    private Vector2 _startDragPosition;
    private bool _isDragMode = false;
    private bool _isScrollMode = false;
    private const float DIRECTION_THRESHOLD = 20f;
    #endregion

    #region Setup and Configuration
    [Inject]
    public void Construct(IDragService dragService)
    {
        _dragService = dragService;
    }

    public void Setup(CubeColor color)
    {
        _color = color;
    }
    
    public void SetScrollRect(ScrollRect parentScrollRect)
    {
        _parentScrollRect = parentScrollRect;
    }
    #endregion

    #region Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        InitializeDragState(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ShouldDetermineDragDirection())
        {
            DetermineDragDirection(eventData);
        }
        
        ExecuteDragAction(eventData);
    }   

    public void OnEndDrag(PointerEventData eventData)
    {
        FinalizeDragAction(eventData);
        ResetDragState();
    }
    
    private void InitializeDragState(Vector2 startPosition)
    {
        _startDragPosition = startPosition;
        _isDragMode = false;
        _isScrollMode = false;
    }
    
    private bool ShouldDetermineDragDirection()
    {
        return !_isDragMode && !_isScrollMode;
    }
    
    private void DetermineDragDirection(PointerEventData eventData)
    {
        Vector2 dragDelta = eventData.position - _startDragPosition;
        
        if (dragDelta.magnitude <= DIRECTION_THRESHOLD)
            return;
            
        if (IsHorizontalMovement(dragDelta))
        {
            EnableScrollMode(eventData);
        }
        else
        {
            EnableDragMode();
        }
    }
    
    private bool IsHorizontalMovement(Vector2 dragDelta)
    {
        float horizontalRatio = Mathf.Abs(dragDelta.x) / dragDelta.magnitude;
        float verticalRatio = Mathf.Abs(dragDelta.y) / dragDelta.magnitude;
        
        return horizontalRatio > verticalRatio;
    }
    
    private void EnableScrollMode(PointerEventData eventData)
    {
        _isScrollMode = true;
        _parentScrollRect?.OnBeginDrag(eventData);
    }
    
    private void EnableDragMode()
    {
        _isDragMode = true;
        _dragService?.SetGhostCubeActive(true);
        _dragService?.SetGhostCubeSprite(_color);
    }
    
    private void ExecuteDragAction(PointerEventData eventData)
    {
        if (_isDragMode)
        {
            _dragService?.MoveGhost(eventData);
        }
        else if (_isScrollMode)
        {
            _parentScrollRect?.OnDrag(eventData);
        }
    }
    
    private void FinalizeDragAction(PointerEventData eventData)
    {
        if (_isDragMode)
        {
            _dragService?.SetGhostCubeActive(false);
            _dragService?.DropCubeToTower(eventData, _color);
        }
        else if (_isScrollMode)
        {
            _parentScrollRect?.OnEndDrag(eventData);
        }
    }
    
    private void ResetDragState()
    {
        _isDragMode = false;
        _isScrollMode = false;
    }
    #endregion
} 