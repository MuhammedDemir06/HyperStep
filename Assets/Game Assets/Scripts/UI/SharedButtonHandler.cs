using UnityEngine;
using UnityEngine.UI;
public class SharedButtonHandler : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    private void Awake()
    {
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }
    private void OnMenuClicked()
    {
        SceneTransitionManager.Instance.LoadScene(SceneType.Menu);
    }
    private void OnRestartClicked()
    {
        SceneTransitionManager.Instance.LoadScene(SceneType.Game);
    }
}