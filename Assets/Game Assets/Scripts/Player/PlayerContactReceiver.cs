using UnityEngine;

public class PlayerContactReceiver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<IContact>()?.OnContact(gameObject);
    }
}
