using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [Header("Take Damage")]
    [Range(.1f,1)]
    [SerializeField] private float damageAmount = .5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newDamage = collision.GetComponent<IHealth>();

        if (collision != null && newDamage != null)
        {
            newDamage.TakeDamage(damageAmount);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var newDamage = collision.gameObject.GetComponent<IHealth>();

        if (collision != null && newDamage != null)
        {
            newDamage.TakeDamage(damageAmount);
        }
    }
}
