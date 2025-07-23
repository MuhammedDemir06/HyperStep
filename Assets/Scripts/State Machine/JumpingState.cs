using UnityEngine;

public class JumpingState : IState
{
    public void EnterState(PlayerController player)
    {
      //  Debug.Log("Entered Jumping State");

        player.PlayerAnim.SetTrigger("Jump");
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
                player.ChangeState(new WalkingState());
            }
            else
            {
                player.ChangeState(new IdleState());
            }
        }
        player.PlayerAnim.SetBool("IsGrounded", player.IsGrounded());
    }
}
