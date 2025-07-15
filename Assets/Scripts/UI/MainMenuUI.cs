using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu UI")]
    [Space(10)]
    [Header("Blur Image")]
    [SerializeField] private Image blurImage;

    private void Start()
    {
        blurImage.gameObject.SetActive(true);
    }

    //Buttons
    public void PlayButton()
    {
        SceneTransitionManager.Instance.LoadScene("Game");
    }
}
