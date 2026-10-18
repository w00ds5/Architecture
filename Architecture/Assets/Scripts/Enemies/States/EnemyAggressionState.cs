public sealed class EnemyAggressionState : IState
{
    private readonly EnemyTargetSensor _sensor;
    private readonly IEnemyMovementBehaviour _movement;

    public EnemyAggressionState(
        EnemyTargetSensor sensor,
        IEnemyMovementBehaviour movement)
    {
        _sensor = sensor;
        _movement = movement;
    }

    public void Enter()
    {
    }

    public void Tick(float deltaTime)
    {
        _movement.TickMovement(_sensor.VisibleTarget);
    }

    public void Exit()
    {
    }
}