using UnityEngine;
using UnityEngine.UI;
 
public interface ICubeFactory
{
    GameObject CreateInventoryCube(CubeColor color, ScrollRect scrollRect, Transform parent);
    GameObject CreateTowerCube(CubeData cubeData, Transform parent);
} 