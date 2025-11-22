using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject transitionUIPrefab;

    private void Awake()
    {
        if (transform.parent != null)
            transform.parent = null;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SpawnTransitionUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void SpawnTransitionUI()
    {
        GameObject transitionUI = Instantiate(transitionUIPrefab);
        DontDestroyOnLoad(transitionUI);
    }
}
