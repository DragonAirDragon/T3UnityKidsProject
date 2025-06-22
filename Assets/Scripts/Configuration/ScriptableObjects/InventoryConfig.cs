using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/InventoryConfig", order = 1)]
public class InventoryConfig : ScriptableObject
{
    #region Color Configuration
    
    [BoxGroup("Available Colors")]
    [ListDrawerSettings(ShowIndexLabels = true, ShowPaging = false, ShowItemCount = true)]
    [ValidateInput("ValidateUniqueColors", "Duplicate colors found!")]
    [OnValueChanged("OnAvailableColorsChanged")]
    public CubeColor[] availableCubeColors;
    
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