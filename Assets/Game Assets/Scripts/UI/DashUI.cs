using DG.Tweening;
using IronTools.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    [ShowDivider(EditorColor.Yellow, "Dash UI")]
    [SerializeField] private Image dashParentImage;
    [SerializeField] private Image dashBarImage;
    private AnimatedPanel dashPanel;

    private IPlayer _playerController;

    private Tween fillTween;
    private Tween idleTween;
    private bool isReadyAnimating;
    private void Awake()
    {
        dashPanel = GetComponent<AnimatedPanel>();
    }
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
        _playerController.OnDashCooldownChanged += OnDashCooldownChanged;
    }
    private void OnDashCooldownChanged(float percent)
    {
        fillTween?.Kill();

        fillTween = dashBarImage
            .DOFillAmount(percent, 0.08f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                    PlayReadyAnimation();
            });

        if (percent < 1f)
            StopReadyAnimation();
    }
    private void PlayReadyAnimation()
    {
        if (isReadyAnimating)
            return;

        isReadyAnimating = true;

        idleTween?.Kill();

        dashParentImage.transform.DOPunchScale(Vector3.one * .15f, .25f);

        dashParentImage.transform.localEulerAngles = new Vector3(0, 0, -5f);

        idleTween = dashParentImage.transform
            .DORotate(new Vector3(0, 0, 5f), .4f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    private void StopReadyAnimation()
    {
        if (!isReadyAnimating)
            return;

        isReadyAnimating = false;

        idleTween?.Kill();

        dashParentImage.transform.DORotate(Vector3.zero, 0.2f);
        dashParentImage.transform.DOScale(Vector3.one, 0.2f);
    }
    public void Show() => dashPanel.Show();
    public void Hide() => dashPanel.Hide();
}
