using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerContactReceiver))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerInstaller : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private PlayerAnimationController playerAnimationController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
    }
    public void PlayerConstructs(IGameStateService gameStateService, IInputProvider inputProvider)
    {
        if (playerController == null || playerHealth == null)
            Debug.LogError("Null Component!");

        playerController.Construct(inputProvider, gameStateService,playerAnimationController);
        playerHealth.Construct(gameStateService,playerController);
    }
    public IHealth Health => playerHealth;
}
