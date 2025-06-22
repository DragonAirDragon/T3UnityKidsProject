using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Config for EffectService
/// </summary>
[CreateAssetMenu(fileName = "AnimationsConfig", menuName = "Configs/AnimationsConfig")]
public class AnimationsConfig : ScriptableObject
{
    [BoxGroup("Cube Dissolve")]
    public float cubeDissolveDuration = 0.5f;

    [BoxGroup("Cube Falling From Tower")]
    public float cubeFallHorizontalDuration = 0.5f;
    public float cubeFallHorizontalDistance = 100f;

    [BoxGroup("Cube Falling To Hole")]
    public float cubeFallToHoleDuration = 0.5f;
    public float cubeFallToHoleDistance = 1000f;

}