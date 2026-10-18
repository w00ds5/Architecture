public sealed class EnemyIdleState : IState
{
    private readonly IEnemyMovementBehaviour _movement;

    public EnemyIdleState(IEnemyMovementBehaviour movement)
    {
        _movement = movement;
    }

    public void Enter()
    {
    }

    public void Tick(float deltaTime)
    {
        _movement.Stop();
    }

    public void Exit()
    {
    }
}