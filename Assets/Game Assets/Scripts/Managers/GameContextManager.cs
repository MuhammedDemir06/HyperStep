using UnityEngine;

public class GameContextManager : MonoBehaviour
{
    private InputManager _newInputManager;
    private IGameStateService _gameStateService;
    [Header("Referances")]
    [SerializeField] private PlayerInstaller playerInstaller;
    [SerializeField] private LevelLoadManager levelLoadManager;

    [SerializeField] private GameUIInstaller gameUIInstaller;
    [SerializeField] private TimeManager timeManager;

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
        gameUIInstaller.MainConstruct(_gameStateService, playerInstaller.Health, playerInstaller.Controller,timeManager);

        playerInstaller.PlayerConstructs(_gameStateService, _newInputManager);

        levelLoadManager.Construct(_gameStateService , playerInstaller.Controller);
        timeManager.Construct(levelLoadManager,_gameStateService);
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