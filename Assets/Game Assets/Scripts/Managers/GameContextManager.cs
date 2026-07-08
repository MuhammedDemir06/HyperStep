using UnityEngine;

public class GameContextManager : MonoBehaviour
{
    private InputManager _newInputManager;
    private IGameStateService _gameStateService;
    [Header("Referances")]
    [SerializeField] private PlayerInstaller playerInstaller;
    [SerializeField] private LevelLoadManager levelLoadManager;

    [SerializeField] private GameUIInstaller gameUIInstaller;

    private void OnEnable()
    {
        //Creating
        _gameStateService = GameManager.Instance._gameStateService;

        _newInputManager = new InputManager();

        _newInputManager.NewInputService();

        _gameStateService.Construct(_newInputManager);
    }
    private void Start()
    {
        //wiring
        playerInstaller.PlayerConstructs(_gameStateService, _newInputManager);

        levelLoadManager.Construct(_gameStateService);

        gameUIInstaller.MainConstruct(_gameStateService, playerInstaller.Health);
    }
    private void OnDisable()
    {
        _newInputManager.NewInputServiceDisable();
    }
    private void Update()
    {
        _newInputManager.UpdateInputX();
    }
}