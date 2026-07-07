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
    private void Start()
    {
        playerHealth = playerMaxHealth;
    }
    public void Construct(IGameStateService gameStateService,PlayerController playerController)
    {
        _gameStateService = gameStateService;
        _playerController = playerController;
    }
    public void TakeDamage(float damageAmount)
    {
        playerHealth -= damageAmount;

        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

        if (playerHealth <= 0)
        {
            _gameStateService.ChangeState(GameState.Death);
            _playerController.ChangePlayerState(new DeathState());
        }

        OnHealthChanged?.Invoke(playerHealth);
    }
    public void TakeHeal(float healAmount)
    {
        playerHealth += healAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

        OnHealthChanged?.Invoke(playerHealth);
    }
}