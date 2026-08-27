using UnityEngine;
using UnityEngine.Audio;

public class GameContextManager : MonoBehaviour
{
    private InputManager _newInputManager;
    private IGameStateService _gameStateService;
    private IAudioStateService _audioStateService;
    private LevelProgressService _levelProgressService;
    private SceneTransitionManager _sceneTransitionManager;
    [Header("Referances")]
    [SerializeField] private PlayerInstaller playerInstaller;
    [SerializeField] private LevelLoadManager levelLoadManager;

    [SerializeField] private GameUIInstaller gameUIInstaller;
    [SerializeField] private TimeManager timeManager;

    private void OnEnable()
    {
        //Creating
        _gameStateService = GameManager.Instance._gameStateService;
        _audioStateService = GameManager.Instance._audioStateService;
        _levelProgressService = GameManager.Instance._levelProgressService;
        _sceneTransitionManager = GameManager.Instance._sceneTransitionManager;

        _newInputManager = new InputManager();

        _newInputManager.NewInputService();

        _gameStateService.Construct(_newInputManager,_levelProgressService);
    }
    private void Start()
    {
        _audioStateService.UpdateMixer();

        //wiring
        gameUIInstaller.MainConstruct(_gameStateService, playerInstaller.Health, playerInstaller.Controller,timeManager,_audioStateService,_sceneTransitionManager,_levelProgressService);

        playerInstaller.PlayerConstructs(_gameStateService, _newInputManager);

        levelLoadManager.Construct(_gameStateService , playerInstaller.Controller,_levelProgressService);

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