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
    [SerializeField] private float lowHealthBound = .3f;

    [Header("Animation Settings")]
    [SerializeField] private float fillDuration = 0.3f;
    [SerializeField] private Ease fillEase = Ease.OutQuad;
    [SerializeField] private float colorTransitionDuration = .3f;
    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthBar;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthBar;
    }
    private void UpdateHealthBar(float healthRatio)
    {
        healthBar.DOFillAmount(healthRatio, fillDuration).SetEase(fillEase);

        Color targetColor = healthRatio < lowHealthBound ? lowHealthColor : normalColor;
        healthBar.DOColor(targetColor, colorTransitionDuration);
    }
}
