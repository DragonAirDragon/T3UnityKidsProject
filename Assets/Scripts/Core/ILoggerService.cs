using System;

public interface ILoggerService
{
    void LogCubePlaced(CubeColor color, int order, float offset);
    void LogCubeRemoved(CubeColor color, int order);
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