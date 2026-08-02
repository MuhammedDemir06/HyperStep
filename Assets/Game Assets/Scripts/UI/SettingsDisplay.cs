using IronTools.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDisplay : MonoBehaviour
{
    [ShowDivider(EditorColor.Green, "Setting UI")]
    [SerializeField] private Button backButton;
    private AnimatedPanel panel;

    private void Awake()
    {
        panel = GetComponent<AnimatedPanel>();
    }
    private void Start()
    {
        backButton.onClick.AddListener(Back);
    }
    public void Back()
    {
        Hide();
    }
    public void Show() => panel.Show();
    public void Hide() => panel.Hide();
}
