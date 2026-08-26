using UnityEngine;

public interface ILevelProgress
{
    CurrentLevelStats CurrentLevel { get; }
    void SetCurrentLevel(CurrentLevelStats currentLevelStats);
    void LevelCompleted();
}

public class LevelProgressService : ILevelProgress
{
    private readonly PlayerDataService _playerDataService;
    private readonly SceneTransitionManager _sceneTransitionManager;

    private CurrentLevelStats currentLevelStats;

    public CurrentLevelStats CurrentLevel => currentLevelStats;

    public LevelProgressService(PlayerDataService playerDataService,SceneTransitionManager sceneTransitionManager)
    {
        _playerDataService = playerDataService;
    }

    public void SetCurrentLevel(CurrentLevelStats currentLevel)
    {
        currentLevelStats = currentLevel;
        Debug.Log("Adjusted");
    }
    public void LevelCompleted()
    {
        var data = _playerDataService.CurrentPlayerData;
        if (currentLevelStats.CurrentChapterIndex != data.CurrentChapter || currentLevelStats.CurrentLevelIndex != data.CurrentLevel)
            return;

        if (currentLevelStats.CurrentLevelIndex + 1 < currentLevelStats.TotalLevelsInChapter)
        {
            data.CurrentLevel++;
            currentLevelStats.CurrentLevelIndex++;
            _sceneTransitionManager.LoadScene(SceneType.Game);
        }
        else if (currentLevelStats.CurrentChapterIndex + 1 < currentLevelStats.TotalChapters)
        {
            data.CurrentChapter++;
            data.CurrentLevel = 0;
            _sceneTransitionManager.LoadScene(SceneType.Menu);
        }
        else
        {
            _sceneTransitionManager.LoadScene(SceneType.Menu);
            Debug.Log("Game fully completed!");
        }

        Debug.Log("Level Completed!");
    }
}
public class CurrentLevelStats
{
    public int CurrentChapterIndex;
    public int CurrentLevelIndex;
    public int TotalLevelsInChapter;
    public int TotalChapters;
}