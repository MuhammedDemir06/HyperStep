using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IHealth
{
    [Header("Player Health")]
    [Space(10)]
    [Range(1, 100)]
    [SerializeField] private float playerMaxHealth = 100;
    private float playerHealth;

    public event Action<float> OnHealthChanged;

    private IGameStateService _gameStateService;
    private PlayerController _playerController;
    private PlayerEffects _playerEffects;
    private PlayerCameraShake _playerShake;
    private void Start()
    {
        playerHealth = playerMaxHealth;
    }
    public void Construct(IGameStateService gameStateService,PlayerController playerController,PlayerEffects playerEffects,PlayerCameraShake playerCameraShake)
    {
        _gameStateService = gameStateService;
        _playerController = playerController;
        _playerEffects = playerEffects;
        _playerShake = playerCameraShake;
    }
    public void TakeDamage(float damageAmount)
    {
        playerHealth -= damageAmount;

        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

        if (playerHealth <= 0)
        {
            _gameStateService.ChangeState(GameState.Death);
            _playerController.ChangePlayerState(_playerController.NewDeathState);
            _playerShake.TriggerShake(CameraShakeType.Hard);
        }

        OnHealthChanged?.Invoke(playerHealth);
        _playerEffects.TakeDamage();
    }
    public void TakeHeal(float healAmount)
    {
        playerHealth += healAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

        OnHealthChanged?.Invoke(playerHealth);
    }
}