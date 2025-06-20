using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public interface IDraggableCube
{
    void Setup(CubeColor color);
    CubeColor Color { get; }
} 