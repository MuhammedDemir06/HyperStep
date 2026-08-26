using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Image LockImage;
    public Button LevelButton;
    [SerializeField] private TextMeshProUGUI levelText;

    public void SetLevel(int level)
    {
        levelText.text = level.ToString();
    }
}
