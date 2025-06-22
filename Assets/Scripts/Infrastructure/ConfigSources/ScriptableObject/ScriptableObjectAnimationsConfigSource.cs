using Cysharp.Threading.Tasks;

/// <summary>
/// Animations configuration source from ScriptableObject
/// </summary>
public class ScriptableObjectAnimationsConfigSource : IAnimationsConfigSource
{
    private readonly AnimationsConfig _scriptableObject;

    public string SourceName => "ScriptableObject";

    public ScriptableObjectAnimationsConfigSource(AnimationsConfig scriptableObject)
    {
        _scriptableObject = scriptableObject;
    }

    public UniTask<(bool success, AnimationsConfigData data)> LoadAsync()
    {
        if (_scriptableObject == null)
        {
            return UniTask.FromResult((false, (AnimationsConfigData)null));
        }

        // Map ScriptableObject to DTO
        var data = new AnimationsConfigData
        {
            cubeDissolveDuration = _scriptableObject.cubeDissolveDuration,
            cubeFallHorizontalDuration = _scriptableObject.cubeFallHorizontalDuration,
            cubeFallHorizontalDistance = _scriptableObject.cubeFallHorizontalDistance,
            cubeFallToHoleDuration = _scriptableObject.cubeFallToHoleDuration,
            cubeFallToHoleDistance = _scriptableObject.cubeFallToHoleDistance
        };

        return UniTask.FromResult((data.IsValid(), data));
    }
}