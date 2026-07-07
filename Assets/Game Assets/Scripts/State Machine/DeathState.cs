using UnityEngine;

public class DeathState : IState
{
    public void EnterState(PlayerController player)
    {
        player.IsDead = true;
        player.Rb.simulated = false;
        player.PlayerAnimation.Death();
    }
    public void ExitState(PlayerController player)
    {
        //Null For Now
    }
    public void UpdateState(PlayerController player)
    {
        //Null For Now
    }
}
