using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class TowerCubeView : MonoBehaviour, IDraggableCube,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CubeColor _color;
    private DragService _dragService;
    private int _cubeIndex; // Индекс куба в башне

    public CubeColor Color => _color;
    public int CubeIndex => _cubeIndex;

    [Inject]
    public void Construct(DragService dragService)
    {
        _dragService = dragService;
    }

    public void Setup(CubeColor color)
    {
        _color = color;
    }

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
} 