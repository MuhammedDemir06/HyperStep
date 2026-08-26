using UnityEngine;
public class GameUIController : MonoBehaviour
{
    [Header("UI Sub-Systems")]
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private LoseUI gameLoseUI;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private LevelTimerUI levelTimerUI;
    [SerializeField] private SettingsDisplay settingsDisplay;
    [SerializeField] private WinUI winDisplay;

    private IGameStateService _gameStateService;
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= OnStateChanged;
    }
    public void Construct(IGameStateService gameStateService)
    {
        _gameStateService = gameStateService;

        _gameStateService.OnStateChanged += OnStateChanged;
    }
    private void OnStateChanged(GameState currentState)
    {
        switch (currentState)
        {
            case GameState.Paused:
                ShowPauseMode();
                break;
            case GameState.GamePlay:
                ShowGameplayMode();
                break;
            case GameState.Death:
                ShowDeathMode();
                break;
            case GameState.Finished:
                ShowWinMode();
                break;
        }

        //
        pauseUI.SettingsButton.onClick.AddListener(settingsDisplay.Show);
        gameLoseUI.SettingsButton.onClick.AddListener(settingsDisplay.Show);
        winDisplay.SettingsButton.onClick.AddListener(settingsDisplay.Show);
    }
    private void ShowGameplayMode()
    {
        healthUI.Show();
        pauseUI.Hide();
        dashUI.Show();
        levelTimerUI.Show();
    }
    private void ShowPauseMode()
    {
        healthUI.Hide();
        pauseUI.Show();
        dashUI.Hide();
        levelTimerUI.Hide();
    }
    private void ShowDeathMode()
    {
        dashUI.Hide();
        healthUI.Hide();
        pauseUI.Hide();
        gameLoseUI.Show();
        levelTimerUI.Hide();
    }
    private void ShowWinMode()
    {
        healthUI.Hide();
        dashUI.Hide();
        levelTimerUI.Hide();
        winDisplay.Show();
    }
}
