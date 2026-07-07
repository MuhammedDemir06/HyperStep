using UnityEngine;

public class LoseUI : MonoBehaviour
{
    private AnimatedPanel loseDisplay;
    private void Awake()
    {
        loseDisplay = GetComponent<AnimatedPanel>();
    }
    public void Show() => loseDisplay.Show();
    public void Hide() => loseDisplay.Hide();
}
