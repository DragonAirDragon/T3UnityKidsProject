using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;


public sealed class DragService
{
    private readonly GameConfig _gameConfig;
    private readonly GhostCubeView _ghostCubeView;
    private readonly RectTransform _towerArea;
    private readonly RectTransform _holeArea;
    private readonly RectTransform _holeParent;
    private readonly TowerService _towerService;
    private readonly ILoggerService _loggerService;
    private readonly EffectsService _effectsService;

    [Inject]
    public DragService( GameConfig gameConfig, GhostCubeView ghostCubeView,TowerService towerService ,RectTransform towerArea, RectTransform holeArea, RectTransform holeParent, ILoggerService loggerService, EffectsService effectsService)
    {
        _gameConfig = gameConfig;
        _ghostCubeView = ghostCubeView;
        _towerService = towerService;
        _towerArea = towerArea;
        _holeArea = holeArea;
        _holeParent = holeParent;
        _loggerService = loggerService;
        _effectsService = effectsService;
        SetGhostCubeActive(false);
    }

    public void SetGhostCubeActive(bool active)
    {
        _ghostCubeView.gameObject.SetActive(active);
    }
    /// <summary>
    /// Размещение куба из инвентаря только в башню
    /// </summary>
    public void DropCubeToTower(PointerEventData e, CubeColor color)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _towerArea, e.position, Camera.main, out var towerLocal) &&
           IsPointInRect(_towerArea.rect, towerLocal))
        {
            _towerService.PlaceCube(color, towerLocal, _towerArea, e);
        }
        else
        {
            Debug.Log("DropCubeToTower: Invalid Area - куб из инвентаря можно размещать только в башне");
            _effectsService?.PlayCubeDissolveEffect(color, e.position);
            _loggerService?.LogCubeIncorrectZone(color);   
        }
    }

    /// <summary>
    /// Удаление куба из башни только в hole
    /// </summary>
    public void DropCubeToHole(PointerEventData e, CubeColor color, int cubeIndex)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _holeArea, e.position, Camera.main, out var holeLocal) &&
            IsPointInRect(_holeArea.rect, holeLocal))
        {
            _effectsService?.PlayCubeFallEffect(color, e.position, EffectDirection.Down, _holeParent, 1000);
            _towerService.RemoveCubeAtIndex(cubeIndex);
        }
        else
        {
            _effectsService?.PlayCubeDissolveEffect(color, e.position);
            _loggerService?.LogCubeIncorrectZone(color);
        }
    }

    private bool IsPointInRect(Rect rect, Vector2 localPoint)
    {
        return localPoint.x >= rect.xMin && localPoint.x <= rect.xMax &&
               localPoint.y >= rect.yMin && localPoint.y <= rect.yMax;
    }

    public void SetGhostCubeSprite(CubeColor color)
    {
        _ghostCubeView.Setup(_gameConfig.GetSpriteForCubeColor(color));
    }
    public void MoveGhost(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _ghostCubeView.ParentRectTransform, e.position, Camera.main, out var local);
        _ghostCubeView.Rect.anchoredPosition = local;
    }
}