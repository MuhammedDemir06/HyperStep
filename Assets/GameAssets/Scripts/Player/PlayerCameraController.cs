using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    private Transform playerCamera;
    private void Start()
    {
        playerCamera = Camera.main.transform.root;

        if (playerCamera == null)
        {
            Debug.LogError("playerCamera not Found!");
        }
    }
    private void Update()
    {
        if (playerCamera == null)
            return;

        Vector3 desiredPosition = transform.position + offset;

        playerCamera.position = Vector3.Lerp(playerCamera.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}