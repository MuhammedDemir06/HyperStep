using System.Collections.Generic;
using UnityEngine;

public class GameUIInstaller : MonoBehaviour
{
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private WinUI winUI;
    [SerializeField] private LevelTimerUI levelTimerUI;
    [SerializeField] private SettingsDisplay settingsDisplay;
    [SerializeField] private List<SharedButtonHandler> buttonHandlers = new();

    public void MainConstruct(IGameStateService gameStateService, IHealth health,PlayerController playerController,TimeManager timeManager,IAudioStateService audioStateService,SceneTransitionManager sceneTransitionManager,LevelProgressService levelProgress)
    {
        healthUI.Construct(health);
        gameUIController.Construct(gameStateService);
        dashUI.Construct(playerController);
        levelTimerUI.Construct(timeManager);
        winUI.Construct(levelProgress);
        settingsDisplay.Construct(audioStateService);

        foreach (var handler in buttonHandlers)
            handler.Construct(sceneTransitionManager);
    }
}
