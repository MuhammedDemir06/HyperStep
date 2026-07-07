using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState DefaultState;

    public IGameStateService _gameStateService;

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

            CreateNewGameStateService().ChangeState(DefaultState);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private IGameStateService CreateNewGameStateService()
    {
        _gameStateService = new GameStateService();
        return _gameStateService;
    }
    private void SpawnTransitionUI()
    {
        GameObject transitionUI = Instantiate(transitionUIPrefab);
        DontDestroyOnLoad(transitionUI);
    }
}
