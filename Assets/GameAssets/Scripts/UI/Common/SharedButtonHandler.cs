using UnityEngine;
using UnityEngine.UI;
public class SharedButtonHandler : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;

    private SceneTransitionManager _sceneTransitionManager;
    private void Awake()
    {
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }
    public void Construct(SceneTransitionManager transitionManager)
    {
        _sceneTransitionManager = transitionManager;
    }
    private void OnMenuClicked()
    {
        _sceneTransitionManager.LoadScene(SceneType.Menu);
    }
    private void OnRestartClicked()
    {
        _sceneTransitionManager.LoadScene(SceneType.Game);
    }
}