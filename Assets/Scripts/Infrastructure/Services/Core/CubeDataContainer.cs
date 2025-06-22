using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeDataContainer : ICubeDataContainer
{
    private List<CubeData> _cubes = new();

    public IReadOnlyList<CubeData> Cubes => _cubes;
    public int Count => _cubes.Count;
    
    public event Action<List<CubeData>> OnDataChanged;

    public void LoadData(List<CubeData> cubes)
    {
        if (cubes?.Count > 0)
        {
            _cubes.Clear();
            for (int i = 0; i < cubes.Count; i++)
            {
                var cube = cubes[i];
                cube.yOffset = 0f;
                cubes[i] = cube;
            }
            
            _cubes.AddRange(cubes);
            NotifyDataChanged();    
        }

    }

    public void AddCube(CubeData cubeData)
    {
        _cubes.Add(cubeData);
        NotifyDataChanged();
    }
    public void AddCubeWithoutNotify(CubeData cubeData)
    {
        _cubes.Add(cubeData);
    }
    public bool RemoveCubeAt(int index)
    {
        if (!IsValidIndex(index))
            return false;
            
        _cubes.RemoveAt(index);
        NotifyDataChanged();
        return true;
    }



    public bool RemoveCubesAt(List<int> indices, bool notify = true)
    {
        if (indices == null || indices.Count == 0)
            return false;

        var sortedIndices = indices.Where(IsValidIndex).OrderByDescending(x => x).ToList();
        
        if (sortedIndices.Count == 0)
            return false;

        foreach (var index in sortedIndices)
        {
            _cubes.RemoveAt(index);
        }
        
        if (notify)
            NotifyDataChanged();
        return true;
    }

    public CubeData GetCubeAt(int index)
    {
        if (!IsValidIndex(index))
            throw new ArgumentOutOfRangeException(nameof(index));
            
        return _cubes[index];
    }

    public int GetCubeIndex(CubeData cubeData)
    {
        return _cubes.IndexOf(cubeData);
    }

    public void UpdateCubeAt(int index, CubeData cubeData)
    {
        if (!IsValidIndex(index))
            throw new ArgumentOutOfRangeException(nameof(index));
            
        _cubes[index] = cubeData;
        NotifyDataChanged();
    }

    public void Clear()
    {
        _cubes.Clear();
        NotifyDataChanged();
    }

    public void ClearGravityEffects()
    {
        for (int i = 0; i < _cubes.Count; i++)
        {
            var cube = _cubes[i];
            if (cube.yOffset != 0f)
            {
                cube.yOffset = 0f;
                _cubes[i] = cube;
            }
        }  
    }

    public void ClearGravityEffect(int index)
    {
        if (!IsValidIndex(index))
            throw new ArgumentOutOfRangeException(nameof(index));
            
        var cube = _cubes[index];
        cube.yOffset = 0f;
        _cubes[index] = cube;
    }

    public bool IsValidIndex(int index)
    {
        if (index >= 0 && index < _cubes.Count)
            return true;
        return false;
    }

    private void NotifyDataChanged()
    {
        OnDataChanged?.Invoke(_cubes);
    }


    public void ApplyGravityEffect(int removedIndex)
    {
        for (int i = removedIndex + 1; i < _cubes.Count; i++)
        {
            var cube = _cubes[i];
            cube.yOffset = 1f;
            _cubes[i] = cube;
        }
    }
} 