using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Button settingsButton;

    private AnimatedPanel panel;
    private void Awake()
    {
        panel = GetComponent<AnimatedPanel>();
    }
    public void Show() =>panel.Show();
    public void Hide() =>panel.Hide();

    public Button SettingsButton => settingsButton;
}
