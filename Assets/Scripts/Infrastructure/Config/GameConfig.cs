using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    #region Color Configuration
    
    [BoxGroup("Available Colors")]
    [ListDrawerSettings(ShowIndexLabels = true, ShowPaging = false, ShowItemCount = true)]
    [ValidateInput("ValidateUniqueColors", "Duplicate colors found!")]
    [OnValueChanged("OnAvailableColorsChanged")]
    public CubeColor[] availableCubeColors;
    
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
    [SerializeField] public float cubeWidth = 150f;
    
    [BoxGroup("Cube Settings")]
    [SerializeField] public float cubeHeight = 150f;
    
    #endregion
    
    #region Editor Utilities
    
    /// <summary>
    /// Adds all available colors from CubeColor enum to the availableCubeColors array
    /// </summary>
    [BoxGroup("Utilities")]
    [HorizontalGroup("Utilities/Buttons")]
    [Button("Add All Colors", ButtonSizes.Medium)]
    [GUIColor(0.4f, 0.8f, 1f)]
    private void AddAllColorsToAvailable()
    {
        availableCubeColors = System.Enum.GetValues(typeof(CubeColor)).Cast<CubeColor>().ToArray();
        MarkDirtyInEditor();
    }
    
    /// <summary>
    /// Clears the available colors array
    /// </summary>
    [HorizontalGroup("Utilities/Buttons")]
    [Button("Clear Colors", ButtonSizes.Medium)]
    [GUIColor(1f, 0.6f, 0.6f)]
    private void ClearAvailableColors()
    {
        availableCubeColors = new CubeColor[0];
        MarkDirtyInEditor();
    }
    
    /// <summary>
    /// Synchronizes sprite data with available colors
    /// Creates entries for new colors while preserving existing ones
    /// </summary>
    [BoxGroup("Utilities")]
    [Button("Sync Data", ButtonSizes.Medium)]
    [GUIColor(0.6f, 1f, 0.6f)]
    private void SyncSpriteData()
    {
        if (availableCubeColors == null) return;
        
        var existingData = cubeColorSpriteData?.ToList() ?? 
                          new System.Collections.Generic.List<CubeColorSpriteData>();
        var newData = new System.Collections.Generic.List<CubeColorSpriteData>();
        
        // Create new array only with colors from availableCubeColors
        foreach (var color in availableCubeColors)
        {
            var existing = existingData.FirstOrDefault(x => x.color == color);
            if (existing.color == color)
            {
                // Preserve existing entry with assigned sprite
                newData.Add(existing);
            }
            else
            {
                // Create new entry for new color
                newData.Add(new CubeColorSpriteData { color = color, sprite = null });
            }
        }
        
        cubeColorSpriteData = newData.ToArray();
        MarkDirtyInEditor();
    }
    
    #endregion
    
    #region Validation and Events
    
    /// <summary>
    /// Validates uniqueness of colors in availableCubeColors array
    /// </summary>
    private bool ValidateUniqueColors(CubeColor[] colors)
    {
        if (colors == null) return true;
        return colors.Length == colors.Distinct().Count();
    }
    
    /// <summary>
    /// Validates uniqueness of colors in sprite data
    /// </summary>
    private bool ValidateSpriteData(CubeColorSpriteData[] data)
    {
        if (data == null) return true;
        return data.Length == data.Select(x => x.color).Distinct().Count();
    }
    
    /// <summary>
    /// Called when available colors are changed
    /// </summary>
    private void OnAvailableColorsChanged()
    {
        MarkDirtyInEditor();
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




