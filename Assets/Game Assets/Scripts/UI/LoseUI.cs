using UnityEngine;

public class LoseUI : MonoBehaviour
{
    private AnimatedPanel panel;
    [Header("Pause Manager")]
    [SerializeField] private PauseManager pauseManager;
    [Header("Pause UI")]
    [SerializeField] private PauseUI pauseUI;
    private void Awake()
    {
        panel = GetComponent<AnimatedPanel>();
    }
    public void OnEnable()
    {
        PlayerHealth.OnHealthChanged += HealthChange;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= HealthChange;
    }
    private void HealthChange(float currentHealth)
    {
        if(currentHealth <= 0)
        {
            pauseUI.DisablePauseUI();
            panel.Show();
            pauseManager.PauseGame();

            Debug.LogWarning("Player Dead!");
        }
    }
}
