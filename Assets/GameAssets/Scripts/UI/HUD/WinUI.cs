using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button nextLevelButton;

    private AnimatedPanel panel;
    private LevelProgressService _levelProgress;
    private void Awake()
    {
        panel = GetComponent<AnimatedPanel>();
    }
    public void Construct(LevelProgressService levelProgress)
    {
        _levelProgress = levelProgress;

        nextLevelButton.onClick.AddListener(_levelProgress.LevelCompleted);
    }
    public void Show() =>panel.Show();
    public void Hide() =>panel.Hide();

    public Button SettingsButton => settingsButton;
}
