using UnityEngine;
using UnityEngine.UI;
public class PauseUI : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Button settingsButton;

    private AnimatedPanel pauseDisplay;
    private void Awake()
    {
        pauseDisplay = GetComponent<AnimatedPanel>();
    }
    public void Show() => pauseDisplay.Show();
    public void Hide() => pauseDisplay.Hide();
    public Button SettingsButton => settingsButton;
}