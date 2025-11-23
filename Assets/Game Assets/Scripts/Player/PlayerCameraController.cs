using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    private Camera playerCamera;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        playerCamera = Camera.main;

        if (playerCamera == null)
        {
            Debug.LogError("playerCamera not Found!");
        }
    }
    private void Update()
    {
        if (playerCamera == null)
        {
            return;
        }

        Vector3 desiredPosition = transform.position + offset;

        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}