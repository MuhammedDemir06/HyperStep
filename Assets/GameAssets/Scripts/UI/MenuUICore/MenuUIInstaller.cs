using UnityEngine;

public class MenuUIInstaller : MonoBehaviour
{
    [SerializeField] private SettingsDisplay settingsDisplay;
    [SerializeField] private MenuLevelCreator levelCreator;

    public void Constructs(IAudioStateService audioStateService,PlayerDataService playerDataManager,LevelProgressService levelProgressService,SceneTransitionManager sceneTransitionManager)
    {
        settingsDisplay.Construct(audioStateService);
        levelCreator.Construct(playerDataManager,levelProgressService,sceneTransitionManager);
    }
}
