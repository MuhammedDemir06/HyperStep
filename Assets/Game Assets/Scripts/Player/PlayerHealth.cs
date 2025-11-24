using UnityEngine;

public class PlayerHealth : MonoBehaviour,IHealth
{
    public static System.Action<float> OnHealthChanged;

    [Header("Player Health")]
    [Space(10)]
    [Range(1, 100)]
    [SerializeField] private float playerMaxHealth = 100;
    private float playerHealth;

    private void Start()
    {
        playerHealth = playerMaxHealth;
    }
    public void TakeDamage(float damageAmount)
    {
        playerHealth -= damageAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);

        if (playerHealth == 0)
            Debug.LogWarning("Player Dead!");

        HealthCahnged();
    }
    public void Heal(float healAmount)
    {
        playerHealth += healAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);
        HealthCahnged();
    }
    private void HealthCahnged()
    {
        OnHealthChanged?.Invoke(playerHealth);
    }
}