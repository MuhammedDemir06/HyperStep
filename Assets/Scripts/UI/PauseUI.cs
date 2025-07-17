using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Header("Pause UI")]
    [Space(10)]
    [Header("References")]
    [SerializeField] private AnimatedPanel pauseDisplay;
    [SerializeField] private PauseManager pauseManager;

    [SerializeField] private AnimatedPanel mobileDisplay;
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
        if (isPaused)
            PauseGame();
        else
            ResumeGame();

        Debug.Log($"Pause State: {isPaused}");
    }

    private void PauseGame()
    {
        pauseDisplay.Show();
        pauseManager.PauseGame();
    }

    private void ResumeGame()
    {
        pauseDisplay.Hide();
        mobileDisplay.Show();
        pauseManager.ResumeGame();
    }
}