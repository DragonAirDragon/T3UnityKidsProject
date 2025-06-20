using System.Collections.Generic;

public interface ISaveService
{
    void SaveTowerData(List<CubeData> cubes);
    List<CubeData> LoadTowerData();
    bool HasSavedData();
    void ClearSavedData();
} 