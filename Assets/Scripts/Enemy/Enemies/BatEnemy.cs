public class BatEnemy : EnemyBase
{
    protected override void Update()
    {
        if (!canMove)
            return;

        base.Update();
        base.SearchForPlayer();
    }
}