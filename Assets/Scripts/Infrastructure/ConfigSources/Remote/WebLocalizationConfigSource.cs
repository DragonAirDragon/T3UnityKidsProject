using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Localization configuration source from web server
/// </summary>
public class WebLocalizationConfigSource : ILocalizationConfigSource
{
    private readonly string _url;
    private readonly float _timeoutSeconds;

    public string SourceName => $"Web ({_url})";

    public WebLocalizationConfigSource(string url, float timeoutSeconds = 15f)
    {
        _url = url;
        _timeoutSeconds = timeoutSeconds;
    }

    public async UniTask<(bool success, LocalizationConfigData data)> LoadAsync()
    {
        try
        {
            using var request = UnityWebRequest.Get(_url);
            request.timeout = (int)_timeoutSeconds;

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                return (false, null);
            }

            string json = request.downloadHandler.text;
            
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