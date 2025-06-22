using System;

/// <summary>
/// Pure animations configuration data object (without Unity dependencies)
/// </summary>
[Serializable]
public class AnimationsConfigData
{
    // Cube Dissolve
    public float cubeDissolveDuration = 0.5f;

    // Cube Falling From Tower
    public float cubeFallHorizontalDuration = 0.5f;
    public float cubeFallHorizontalDistance = 100f;

    // Cube Falling To Hole
    public float cubeFallToHoleDuration = 0.5f;
    public float cubeFallToHoleDistance = 1000f;

    /// <summary>
    /// Configuration data validation
    /// </summary>
    public bool IsValid()
    {
        return cubeDissolveDuration > 0 &&
               cubeFallHorizontalDuration > 0 &&
               cubeFallHorizontalDistance > 0 &&
               cubeFallToHoleDuration > 0 &&
               cubeFallToHoleDistance > 0;
    }
}