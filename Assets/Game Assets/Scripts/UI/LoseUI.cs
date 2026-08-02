using UnityEngine;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private Button settingButton;
    private AnimatedPanel loseDisplay;
    private void Awake()
    {
        loseDisplay = GetComponent<AnimatedPanel>();
    }
    public void Show() => loseDisplay.Show();
    public void Hide() => loseDisplay.Hide();
    public Button SettingsButton => settingButton;
}
