using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu UI")]
    [Space(10)]
    [Header("Blur Image")]
    [SerializeField] private Image blurImage;
    [Header("Quit Button")]
    [SerializeField] private Button quitButton;

    private void Start()
    {
        blurImage.gameObject.SetActive(true);

        quitButton.onClick.AddListener(GameQuit);
    }
    private void GameQuit()
    {
        SceneTransitionManager.Instance.QuitGame();
    }
    //Test Button**
    public void PlayButton()
    {
        SceneTransitionManager.Instance.LoadScene("Game");
    }
}