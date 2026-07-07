using UnityEngine;
public class PauseUI : MonoBehaviour
{
    private AnimatedPanel pauseDisplay;
    private void Awake()
    {
        pauseDisplay = GetComponent<AnimatedPanel>();
    }
    public void Show() => pauseDisplay.Show();
    public void Hide() => pauseDisplay.Hide();
}