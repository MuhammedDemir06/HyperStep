using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthUI : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Image healthBar;

    [Header("Colors")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color lowHealthColor;

    [Header("Animation Settings")]
    [SerializeField] private float fillDuration = 0.3f;
    [SerializeField] private Ease fillEase = Ease.OutQuad;
    [SerializeField] private float colorTransitionDuration = .3f;

    private AnimatedPanel healthDisplay;

    private IHealth _healthService;
    private void Awake()
    {
        healthDisplay = GetComponent<AnimatedPanel>();
    }
    public void Construct(IHealth health)
    {
        _healthService = health;
        _healthService.OnHealthChanged += UpdateHealthBar;
    }
    private void UpdateHealthBar(float healthAmount)
    {
        float healthRatio = healthAmount / 100;

        healthBar.DOFillAmount(healthRatio, fillDuration).SetEase(fillEase);

        Color targetColor = healthRatio < 0.3f ? lowHealthColor : normalColor;
        healthBar.DOColor(targetColor, colorTransitionDuration);
    }

    public void Show() => healthDisplay.Show();
    public void Hide() => healthDisplay.Hide();
}
