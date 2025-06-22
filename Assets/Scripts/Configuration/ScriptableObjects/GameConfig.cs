using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    #region Color Sprites
    
    [BoxGroup("Color Sprites")]
    [TableList(ShowIndexLabels = true, ShowPaging = false)]
    [ValidateInput("ValidateSpriteData", "Duplicate colors found in sprite data!")]
    [SerializeField] public CubeColorSpriteData[] cubeColorSpriteData;
    
    public Sprite GetSpriteForCubeColor(CubeColor color)
    {
        return cubeColorSpriteData.First(x => x.color == color).sprite;
    }

    #endregion

    #region Cube Dimensions
    
    [BoxGroup("Cube Settings")]
    public float cubeWidth = 150f;
    [BoxGroup("Cube Settings")]
    public float cubeHeight = 150f;


    [BoxGroup("Placement Settings")]
    public bool isManualPlacement = false;
    
    #endregion
    
    #region Validation
    
    /// <summary>
    /// Validates uniqueness of colors in sprite data
    /// </summary>
    private bool ValidateSpriteData(CubeColorSpriteData[] data)
    {
        if (data == null) return true;
        return data.Length == data.Select(x => x.color).Distinct().Count();
    }
    
    /// <summary>
    /// Marks the object as dirty in Unity Editor
    /// </summary>
    private void MarkDirtyInEditor()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
    
    #endregion
}




