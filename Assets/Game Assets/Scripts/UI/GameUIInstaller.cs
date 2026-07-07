using UnityEngine;

public class GameUIInstaller : MonoBehaviour
{
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private GameUIController gameUIController;

    public void MainConstruct(IGameStateService gameStateService, IHealth health)
    {
        healthUI.Construct(health);
        gameUIController.Construct(gameStateService);
    }
}
