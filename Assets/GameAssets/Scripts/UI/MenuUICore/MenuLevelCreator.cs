using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelCreator : MonoBehaviour
{
    [Header("UI Referances")]
    [SerializeField] private Transform content;
    [SerializeField] private ChapterData chapterData;

    [SerializeField] private GameObject chapterPrefab;
    [SerializeField] private GameObject levelPrefab;

    private PlayerDataService _playerDataService;
    private LevelProgressService _levelProgressService;
    private SceneTransitionManager _sceneTransitionManager;

    private void Start()
    {
        CreateChapters();
    }
    public void Construct(PlayerDataService playerDataManager,LevelProgressService levelProgressService,SceneTransitionManager sceneTransitionManager)
    {
        _playerDataService = playerDataManager;
        _levelProgressService = levelProgressService;
        _sceneTransitionManager = sceneTransitionManager;
    }
    private void CreateChapters()
    {
        int chapterIndex = 0;
        foreach (Chapter chapter in chapterData.Chapters)
        {
            GameObject chapterObject = Instantiate(chapterPrefab, content);
            ChapterUI chapterUI = chapterObject.GetComponent<ChapterUI>();
            chapterUI.SetChapter(chapter);

            int levelIndex = 0;
            int totalLevelsInChapter = chapter.Levels.Count;

            foreach (LevelData level in chapter.Levels)
            {
                GameObject levelObject = Instantiate(levelPrefab, chapterUI.LevelContent);
                LevelUI levelUI = levelObject.GetComponent<LevelUI>();
                levelUI.SetLevel(levelIndex);

                bool isLocked;
                if (chapterIndex < _playerDataService.CurrentPlayerData.CurrentChapter)
                {
                    isLocked = false;
                }
                else if (chapterIndex > _playerDataService.CurrentPlayerData.CurrentChapter)
                {
                    isLocked = true;
                }
                else
                {
                    isLocked = levelIndex > _playerDataService.CurrentPlayerData.CurrentLevel;
                }

                levelUI.LockImage.gameObject.SetActive(isLocked);
                levelUI.LevelButton.interactable = !isLocked;

                var levelStats = new CurrentLevelStats
                { CurrentChapterIndex = chapterIndex, CurrentLevelIndex = levelIndex, TotalChapters = chapterData.Chapters.Count, TotalLevelsInChapter = totalLevelsInChapter };

                levelUI.LevelButton.onClick.RemoveAllListeners();
                levelUI.LevelButton.onClick.AddListener(() =>LevelButton(levelStats, isLocked));

                levelIndex++;
            }
            chapterIndex++;
        }
    }
    public void LevelButton(CurrentLevelStats currentLevel, bool isLocked)
    {
        if (isLocked)
            return;

        _levelProgressService.SetCurrentLevel(currentLevel);
        _sceneTransitionManager.LoadScene(SceneType.Game);
    }
}
