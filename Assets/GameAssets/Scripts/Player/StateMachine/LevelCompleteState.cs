public class LevelCompleteState : IState
{
    public void EnterState(PlayerController player)
    {
        player.SetPlayerMoveState();
    }
    public void ExitState(PlayerController player)
    {
    }

    public void UpdateState(PlayerController player)
    {
    }
}