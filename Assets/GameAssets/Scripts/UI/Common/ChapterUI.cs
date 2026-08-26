using TMPro;
using UnityEngine;

public class ChapterUI : MonoBehaviour
{
    public Transform LevelContent;

    [SerializeField] private TextMeshProUGUI chapterText;

    public void SetChapter(Chapter chapter)
    {
        chapterText.text = chapter.ChapterName;
    }
}
