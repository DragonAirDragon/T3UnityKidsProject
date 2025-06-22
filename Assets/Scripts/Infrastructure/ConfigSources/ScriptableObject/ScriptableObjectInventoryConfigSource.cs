using Cysharp.Threading.Tasks;

/// <summary>
/// Inventory configuration source from ScriptableObject
/// </summary>
public class ScriptableObjectInventoryConfigSource : IInventoryConfigSource
{
    private readonly InventoryConfig _scriptableObject;

    public string SourceName => "ScriptableObject";

    public ScriptableObjectInventoryConfigSource(InventoryConfig scriptableObject)
    {
        _scriptableObject = scriptableObject;
    }

    public UniTask<(bool success, InventoryConfigData data)> LoadAsync()
    {
        if (_scriptableObject == null)
        {
            return UniTask.FromResult((false, (InventoryConfigData)null));
        }

        // Map ScriptableObject to DTO
        var data = new InventoryConfigData
        {
            availableCubeColors = _scriptableObject.availableCubeColors
        };

        return UniTask.FromResult((data.IsValid(), data));
    }
}