using Unity.VisualScripting;
using UnityEngine;

public class JumpingState : IState
{
    public void EnterState(PlayerController player)
    {
        if (player.IsGrounded())
        {
            player.Rb.linearVelocity = new Vector2(player.Rb.linearVelocity.x, player.JumpForce * 10);
            player.PlayerAnimation.Jump();
        }
    }
    public void ExitState(PlayerController player)
    {
       //  Debug.Log("Exited Jumping State");
    }
    public void UpdateState(PlayerController player)
    {
        if (player.IsGrounded())
        {
            if (player.IsWalking)
            {
                player.ChangePlayerState(player.NewWalkingState);
            }
            else
            {
                player.ChangePlayerState(player.NewIdleState);
            }
        }
        player.PlayerAnimation.Grounded(player.IsGrounded());
    }
}
