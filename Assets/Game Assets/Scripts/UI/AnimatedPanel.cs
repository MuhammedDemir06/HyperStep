using DG.Tweening;
using UnityEngine;

public class AnimatedPanel : MonoBehaviour
{
    [Header("Target Panel")]
    [SerializeField] private CanvasGroup targetGroup;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private Ease openEase = Ease.OutCubic;
    [SerializeField] private Ease closeEase = Ease.InCubic;

    [SerializeField] private bool hideOnStart = true;
    private void Start()
    {
        if (targetGroup == null) return;

        targetGroup.DOKill();

        if (hideOnStart)
        {
            targetGroup.alpha = 0f;
            targetGroup.interactable = false;
            targetGroup.blocksRaycasts = false;
        }
        else
        {
            targetGroup.alpha = 1f;
            targetGroup.interactable = true;
            targetGroup.blocksRaycasts = true;
        }
    }
    public void Show()
    {
        if (targetGroup == null) return;

        targetGroup.DOKill();
        targetGroup.DOFade(1f, animationDuration).SetEase(openEase);
        targetGroup.interactable = true;
        targetGroup.blocksRaycasts = true;
    }
    public void Hide()
    {
        if (targetGroup == null) return;

        targetGroup.DOKill();
        targetGroup.DOFade(0f, animationDuration).SetEase(closeEase)
            .OnComplete(() =>
            {
                targetGroup.interactable = false;
                targetGroup.blocksRaycasts = false;
            });
    }
}
