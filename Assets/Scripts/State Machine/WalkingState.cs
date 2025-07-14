using UnityEngine;

public class WalkingState : IState
{
    public void EnterState(PlayerController player)
    {
        Debug.Log("Entered Walking State");
    }
    public void ExitState(PlayerController player)
    {
        Debug.Log("Exited Walking State");
    }
    public void UpdateState(PlayerController player)
    {
        if (!player.IsGrounded())
        {
            player.ChangeState(new JumpingState());
        }
        else if (!player.IsWalking)
        {
            player.ChangeState(new IdleState());
        }
    }
}
