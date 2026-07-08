using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [Header("UI Sub-Systems")]
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private LoseUI gameLoseUI;

    private IGameStateService _gameStateService;

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
    }
    private void ShowPauseMode()
    {
        healthUI.Hide();
        pauseUI.Show();
    }
    private void ShowDeathMode()
    {
        healthUI.Hide();
        pauseUI.Hide();
        gameLoseUI.Show();
    }
}
