using UnityEngine;

public class DamageOnContact : MonoBehaviour,IContact
{
    [SerializeField] private float damage = 25f;

    public void OnContact(GameObject target)
    {
        if (target.TryGetComponent<IHealth>(out var health))
        {
            health.TakeDamage(damage);
        }
    }
}
