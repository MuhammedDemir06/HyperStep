using UnityEngine;

public class HealOnContact : MonoBehaviour,IContact
{
    [SerializeField] private float heal = 25f;

    public void OnContact(GameObject target)
    {
        if (target.TryGetComponent<IHealth>(out var health))
        {
            health.TakeHeal(heal);
        }
    }
}
