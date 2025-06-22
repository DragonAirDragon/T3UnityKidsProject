using System;

[Serializable]
public struct CubeData
{
    public CubeColor color;
    public float offset;

    [NonSerialized]
    public float yOffset;
}