using UnityEngine;

public class GameUIInstaller : MonoBehaviour
{
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private LevelTimerUI levelTimerUI;

    public void MainConstruct(IGameStateService gameStateService, IHealth health,PlayerController playerController,TimeManager timeManager)
    {
        healthUI.Construct(health);
        gameUIController.Construct(gameStateService);
        dashUI.Construct(playerController);
        levelTimerUI.Construct(timeManager);
    }
}
