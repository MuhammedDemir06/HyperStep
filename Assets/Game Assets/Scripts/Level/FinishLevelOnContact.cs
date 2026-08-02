using UnityEngine;

public class FinishLevelOnContact : MonoBehaviour, ILevelInitializable,IContact
{
    private IGameStateService gameStateService;
    public void Initialize(IGameStateService _gameStateService)
    {
        this.gameStateService = _gameStateService;
    }

    public void OnContact(GameObject target)
    {
        gameStateService.ChangeState(GameState.Finished);
    }
}
