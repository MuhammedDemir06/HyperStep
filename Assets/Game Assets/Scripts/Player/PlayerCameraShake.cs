using UnityEngine;
public enum CameraShakeType
{
    Soft,
    Medium,
    Hard
}
public class PlayerCameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    private Coroutine currentShakeCoroutine;
    private WaitForEndOfFrame cachedWaitFrame;

    private Transform playerCamera;

    //
    private readonly float _softDuration = 0.15f;
    private readonly float _softMagnitude = 0.08f;

    private readonly float _mediumDuration = 0.25f;
    private readonly float _mediumMagnitude = 0.2f;

    private readonly float _hardDuration = 0.4f;
    private readonly float _hardMagnitude = 0.45f;

    private void Awake()
    {
        cachedWaitFrame = new WaitForEndOfFrame();
    }
    private void OnEnable()
    {
        originalPos = transform.localPosition;
    }
    private void Start()
    {
        playerCamera = Camera.main.transform;
    }
    public void TriggerShake(CameraShakeType shakeType)
    {
        float duration = 0f;
        float magnitude = 0f;

        switch (shakeType)
        {
            case CameraShakeType.Soft:
                duration = _softDuration;
                magnitude = _softMagnitude;
                break;
            case CameraShakeType.Medium:
                duration = _mediumDuration;
                magnitude = _mediumMagnitude;
                break;
            case CameraShakeType.Hard:
                duration = _hardDuration;
                magnitude = _hardMagnitude;
                break;
        }

        if (currentShakeCoroutine != null)
        {
            StopCoroutine(currentShakeCoroutine);
        }

        currentShakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }
    private System.Collections.IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float currentMagnitude = Mathf.Lerp(magnitude, 0f, elapsed / duration);

            Vector2 randomPoint = Random.insideUnitCircle * currentMagnitude;
            playerCamera.localPosition = new Vector3(originalPos.x + randomPoint.x, originalPos.y + randomPoint.y, originalPos.z);

            yield return cachedWaitFrame;
        }

        playerCamera.localPosition = originalPos;
        currentShakeCoroutine = null;
    }
}
