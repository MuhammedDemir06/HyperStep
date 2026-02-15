using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static Transform Player { get; private set; }

    private void Awake()
    {
        if (Player != null && Player != transform)
        {
            Destroy(gameObject);
            return;
        }

        Player = transform;
    }
}