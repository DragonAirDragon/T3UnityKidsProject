using System;
using System.Collections.Generic;

public interface ICubeDataContainer
{
    IReadOnlyList<CubeData> Cubes { get; }
    int Count {get;}
    event Action<List<CubeData>> OnDataChanged;
    void LoadData(List<CubeData> cubes);
    void AddCube(CubeData cubeData);
    void AddCubeWithoutNotify(CubeData cubeData);
    bool RemoveCubeAt(int index);
    bool RemoveCubesAt(List<int> indices, bool notify = true);
    CubeData GetCubeAt(int index);
    int GetCubeIndex(CubeData cubeData);
    void ApplyGravityEffect(int removedIndex);
    void UpdateCubeAt(int index, CubeData cubeData);
    void ClearGravityEffect(int index);
    void ClearGravityEffects();
    void Clear();
    bool IsValidIndex(int index);
} 