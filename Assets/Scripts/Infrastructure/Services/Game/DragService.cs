using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

/// <summary>
/// Service responsible for dragging cubes
/// </summary>
public sealed class DragService : IDragService
{
    #region Fields and Dependencies
    private readonly GameConfig _gameConfig;
    private readonly GhostCubeView _ghostCubeView;
    private readonly RectTransform _towerArea;
    private readonly RectTransform _holeArea;
    private readonly RectTransform _holeParent;
    private readonly TowerService _towerService;
    private readonly ILoggerService _loggerService;
    private readonly IEffectsService _effectsService;
    #endregion

    [Inject]
    public DragService( GameConfig gameConfig, GhostCubeView ghostCubeView,TowerService towerService ,RectTransform towerArea, RectTransform holeArea, RectTransform holeParent, ILoggerService loggerService, IEffectsService effectsService)
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
 
    public void DropCubeToTower(PointerEventData e, CubeColor color)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _towerArea, e.position, Camera.main, out var towerLocal) &&
           IsPointInRect(_towerArea.rect, towerLocal))
        {
            _towerService.PlaceCube(color, towerLocal, e);
        }
        else
        {
            _effectsService?.PlayCubeWrongZoneEffect(color, e.position);
            _loggerService?.LogCubeIncorrectZone(color);   
        }
    }

    public void DropCubeToHole(PointerEventData e, CubeColor color, int cubeIndex)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _holeArea, e.position, Camera.main, out var holeLocal) &&
            IsPointInRect(_holeArea.rect, holeLocal))
        {
            _effectsService?.PlayCubeSuccessfulRemovalEffect(color, e.position, EffectDirection.Down, _holeParent);
            _towerService.RemoveCubeAtIndex(cubeIndex);
        }
        else
        {
            _effectsService?.PlayCubeWrongZoneEffect(color, e.position);
            _loggerService?.LogCubeIncorrectZone(color);
        }
    }

    /// <summary>
    /// Check if point is in rect (example: tower area, hole area)
    /// </summary>
    /// <param name="rect">Rect</param>
    /// <param name="localPoint">Local point</param>
    /// <returns>True if point is in rect, false otherwise</returns>
    private bool IsPointInRect(Rect rect, Vector2 localPoint)
    {
        return localPoint.x >= rect.xMin && localPoint.x <= rect.xMax &&
               localPoint.y >= rect.yMin && localPoint.y <= rect.yMax;
    }

    public void SetGhostCubeSprite(CubeColor color)
    {
        _ghostCubeView.Setup(_gameConfig.GetSpriteForCubeColor(color));
    }
    /// <summary>
    /// Move ghost cube (screen position to local position for ghost cube view)
    /// </summary>
    /// <param name="e"></param>
    public void MoveGhost(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _ghostCubeView.ParentRectTransform, e.position, Camera.main, out var local);
        _ghostCubeView.Rect.anchoredPosition = local;
    }
}