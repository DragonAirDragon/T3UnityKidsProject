using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveService : ISaveService
{
    private readonly string _saveFileName = "tower.json";
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, _saveFileName);

    public void SaveTowerData(List<CubeData> cubes)
    {
        try
        {
            var saveData = new SaveData { cubes = cubes };
            string jsonData = JsonUtility.ToJson(saveData);
            File.WriteAllText(SaveFilePath, jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Save error: {e.Message}");
        }
    }

    public List<CubeData> LoadTowerData()
    {
        try
        {
            if (!File.Exists(SaveFilePath))
                return new List<CubeData>();

            string jsonData = File.ReadAllText(SaveFilePath);
            var saveData = JsonUtility.FromJson<SaveData>(jsonData);
            
            return saveData?.cubes ?? new List<CubeData>();
        }
        catch (Exception e)
        {
            Debug.LogError($"Load error: {e.Message}");
            return new List<CubeData>();
        }
    }

    public bool HasSavedData()
    {
        return File.Exists(SaveFilePath);
    }

    public void ClearSavedData()
    {
        if (File.Exists(SaveFilePath))
            File.Delete(SaveFilePath);
    }
} 