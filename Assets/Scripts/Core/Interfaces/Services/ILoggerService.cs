using System;

public interface ILoggerService
{
    void LogCubePlaced(CubeColor color, int index, float offset);
    void LogCubeRemoved(CubeColor color, int index);
    void LogCubeIncorrectHeight(CubeColor color);
    void LogCubeIncorrectOffset(CubeColor color);
    void LogCubeIncorrectZone(CubeColor color);
    void LogFloatingCubesRemoved(int count);
    void LogSaveTower();
    void LogLoadTower();
    void ClearLog();
    string GetCurrentLog();
    event Action<string> OnLogUpdated;
} 