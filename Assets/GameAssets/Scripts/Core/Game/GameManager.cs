using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState DefaultState;

    public GameStateService _gameStateService;
    public AudioStateService _audioStateService;
    public PlayerDataService _playerDataService;
    public LevelProgressService _levelProgressService;
    public SceneTransitionManager _sceneTransitionManager;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject transitionUIPrefab;

    private void Awake()
    {
        if (transform.parent != null)
            transform.parent = null;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CreateServices();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void CreateServices()
    {
        if(audioMixer == null)
        {
            Debug.Log("Audio Mixer Not Found!");
            return;
        }

        //GameState
        _gameStateService = new GameStateService();

        //PlayerData
        _playerDataService = new PlayerDataService();
        _playerDataService.LoadData();

        //Scene Transition
        SpawnTransitionUI();

        //Level Progress
        _levelProgressService = new LevelProgressService(_playerDataService,_sceneTransitionManager);

        //Audio
        AudioStateService audioService = new AudioStateService();
        audioService.Construct(audioMixer, _playerDataService);

        _audioStateService = audioService;

        _audioStateService.UpdateMixer();
    }
    private void SpawnTransitionUI()
    {
        GameObject transitionUI = Instantiate(transitionUIPrefab);

        _sceneTransitionManager = transitionUI.GetComponent<SceneTransitionManager>();

        if (_sceneTransitionManager != null)
            _sceneTransitionManager.Construct(_playerDataService);

        DontDestroyOnLoad(transitionUI);
    }
    //Extra Save
    private void OnApplicationQuit()
    {
        _playerDataService.SaveData();
    }
}
