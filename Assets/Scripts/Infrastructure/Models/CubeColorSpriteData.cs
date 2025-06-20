using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// Data structure to link cube color with its sprite
/// </summary>
[System.Serializable]
public struct CubeColorSpriteData
{
    [TableColumnWidth(150, Resizable = false)]
    [LabelWidth(60)]
    public CubeColor color;
    
    [TableColumnWidth(200, Resizable = false)]
    [PreviewField(55, ObjectFieldAlignment.Center)]
    [LabelWidth(60)]
    public Sprite sprite;
}