using UnityEngine;
using DG.Tweening;

public class PlayerDamageEffect : MonoBehaviour
{
    private Color originalColor;

    [Header("Damage Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color damageColor = Color.red;
    public float flashDuration = 0.2f;
    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += TakeDamage;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= TakeDamage;
    }
    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }
    public void TakeDamage(float amount)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(spriteRenderer.DOColor(damageColor, flashDuration));
        seq.Append(spriteRenderer.DOColor(originalColor, flashDuration));
        seq.Play();
    }
}
