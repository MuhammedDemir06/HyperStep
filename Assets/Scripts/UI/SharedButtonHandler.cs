using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneTransitionManager.Instance.LoadScene("Menu");
    }
    private void OnRestartClicked()
    {
        SceneTransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }
}