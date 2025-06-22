using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Localization configuration source from JSON file
/// </summary>
public class JsonFileLocalizationConfigSource : ILocalizationConfigSource
{
    private readonly string _filePath;

    public string SourceName => $"JSON File ({_filePath})";

    public JsonFileLocalizationConfigSource(string filePath)
    {
        _filePath = filePath;
    }

    public async UniTask<(bool success, LocalizationConfigData data)> LoadAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return (false, null);
            }

            string json = await File.ReadAllTextAsync(_filePath);
            
            if (string.IsNullOrEmpty(json))
            {
                return (false, null);
            }

            var data = JsonUtility.FromJson<LocalizationConfigData>(json);
            
            if (data == null || !data.IsValid())
            {
                return (false, null);
            }

            return (true, data);
        }
        catch (System.Exception)
        {
            return (false, null);
        }
    }
}