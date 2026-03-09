using UnityEngine;
public class PauseUI : MonoBehaviour
{
    [Header("Pause UI")]
    [Space(10)]
    [Header("References")]
    [SerializeField] private AnimatedPanel pauseDisplay;
    [SerializeField] private PauseManager pauseManager;

    [SerializeField] private AnimatedPanel healthDisplay;

    private bool disabled = false;
    private void OnEnable()
    {
        InputManager.GamePause += TogglePause;
    }
    private void OnDisable()
    {
        InputManager.GamePause -= TogglePause;
    }
    //Pc
    public void TogglePause(bool isPaused)
    {
        if (disabled)
            return;

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    private void PauseGame()
    {
        healthDisplay.Hide();

        pauseDisplay.Show();
        pauseManager.PauseGame();
    }

    private void ResumeGame()
    {
        healthDisplay.Show();

        pauseDisplay.Hide();

        pauseManager.ResumeGame();
    }
    public void DisablePauseUI()
    {
        disabled = true;
    }
}