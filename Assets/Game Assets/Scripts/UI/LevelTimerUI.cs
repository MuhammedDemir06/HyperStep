using DG.Tweening;
using IronTools.Attributes;
using TMPro;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    [ShowDivider(EditorColor.Green, "Level Timer UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    private AnimatedPanel levelTimerPanel;

    private ITime _timeManager;
    private void Awake()
    {
        levelTimerPanel = GetComponent<AnimatedPanel>();
    }
    public void Construct(TimeManager timeManager)
    {
        _timeManager = timeManager;

        _timeManager.OnTimeChanged += ShowTime;
    }
    private void ShowTime(int currentTime)
    {
        int minutes = currentTime / 60;
        int seconds = currentTime % 60;

        timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);

        if (currentTime <= 10 && currentTime > 0)
        {
            timeText.color = Color.red;

            if (!DOTween.IsTweening(timeText.transform))
            {
                timeText.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 1, 0.5f);
            }
        }
        else if (currentTime > 10)
        {
            timeText.color = Color.white;

            timeText.transform.DOComplete();
            timeText.transform.localScale = Vector3.one;
        }
    }
    public void Show() => levelTimerPanel.Show();
    public void Hide() => levelTimerPanel.Hide();
}
