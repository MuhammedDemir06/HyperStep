using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("UI Sub-Systems")]
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private LoseUI gameLoseUI;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private LevelTimerUI levelTimerUI;

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
        }
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
}
