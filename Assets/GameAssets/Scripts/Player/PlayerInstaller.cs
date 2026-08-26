using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerContactReceiver))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerCameraShake))]
public class PlayerInstaller : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private PlayerAnimationController playerAnimationController;
    private PlayerEffects playerEffects;
    private PlayerCameraShake playerShake;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        playerEffects = GetComponent<PlayerEffects>();
        playerShake = GetComponent<PlayerCameraShake>();
    }
    public void PlayerConstructs(IGameStateService gameStateService, IInputProvider inputProvider)
    {
        if (playerController == null || playerHealth == null)
            Debug.LogError("Null Component!");

        playerController.Construct(inputProvider, gameStateService,playerAnimationController,playerShake);
        playerHealth.Construct(gameStateService, playerController, playerEffects, playerShake);
    }
    public IHealth Health => playerHealth;
    public PlayerController Controller => playerController;
}
