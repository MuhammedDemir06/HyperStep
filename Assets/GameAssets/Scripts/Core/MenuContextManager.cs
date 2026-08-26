using UnityEngine;

public class MenuContextManager : MonoBehaviour
{
    //Services
    private IAudioStateService _audioStateService;
    private PlayerDataService _playerDataManager;
    private LevelProgressService _levelProgressService;
    private SceneTransitionManager _sceneTransitionManager;

    [Header("Referances")]
    [SerializeField] private MenuUIInstaller menuUIInstaller;

    private void OnEnable()
    {
        //Creating
        _audioStateService = GameManager.Instance._audioStateService;
        _playerDataManager = GameManager.Instance._playerDataService;
        _levelProgressService = GameManager.Instance._levelProgressService;
        _sceneTransitionManager = GameManager.Instance._sceneTransitionManager;
    }
    private void Start()
    {
        //wiring
        menuUIInstaller.Constructs(_audioStateService,_playerDataManager,_levelProgressService,_sceneTransitionManager);
    }

}
