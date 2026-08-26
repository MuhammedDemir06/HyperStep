using UnityEngine;

public class FinishLevelOnContact : MonoBehaviour, ILevelInitializable,IContact
{
    private IGameStateService _gameStateService;
    public void Initialize(IGameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    public void OnContact(GameObject target)
    {
        _gameStateService.ChangeState(GameState.Finished);
    }
}
