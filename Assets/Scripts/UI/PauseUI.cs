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

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    private void OnEnable()
    {
        InputManager.GamePause += Pause;
    }
    private void OnDisable()
    {
        InputManager.GamePause -= Pause;
    }
    private void Awake()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
    }
    private void Pause()
    {
        pauseDisplay.Show();
        pauseManager.PauseGame();
    }
    public void Resume()
    {
        pauseDisplay.Hide();
        mobileDisplay.Show();
        pauseManager.ResumeGame();
    }
}
