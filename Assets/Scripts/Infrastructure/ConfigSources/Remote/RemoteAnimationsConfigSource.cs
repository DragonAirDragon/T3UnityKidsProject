using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Animations configuration source from remote server
/// </summary>
public class RemoteAnimationsConfigSource : IAnimationsConfigSource
{
    private readonly string _url;
    private readonly float _timeoutSeconds;

    public string SourceName => $"Remote ({_url})";

    public RemoteAnimationsConfigSource(string url, float timeoutSeconds = 10f)
    {
        _url = url;
        _timeoutSeconds = timeoutSeconds;
    }

    public async UniTask<(bool success, AnimationsConfigData data)> LoadAsync()
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

            var data = JsonUtility.FromJson<AnimationsConfigData>(json);
            
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