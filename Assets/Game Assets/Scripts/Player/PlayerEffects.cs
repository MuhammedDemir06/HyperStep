using UnityEngine;
using DG.Tweening;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Color Settings")]
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor = Color.white;

    private void Start()
    {
        if(spriteRenderer == null)
        {
            Debug.LogError("Null referance spriteRenderer!!");
            return;
        }
        originalColor = spriteRenderer.color;
    }
    public void TakeDamage()
    {
        DOTween.Kill(transform);
        DOTween.Kill(spriteRenderer);
        transform.localScale = Vector3.one;

        spriteRenderer.DOColor(damageColor, 0.05f)
            .SetLoops(4, LoopType.Yoyo)
            .OnComplete(() => spriteRenderer.color = originalColor);

        transform.DOPunchScale(new Vector3(-0.15f, 0.2f, 0f), 0.2f, 8, 0.5f);
    }
}
