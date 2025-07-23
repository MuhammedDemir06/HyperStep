using UnityEngine;

public class SlimeEnemy : EnemyBase
{
    protected override void Update()
    {
        if (!canMove)
            return;

        base.Update();
        base.Patrol();
    }
}
