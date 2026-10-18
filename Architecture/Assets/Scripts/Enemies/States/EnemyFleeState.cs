public sealed class EnemyFleeState : IState
{
    private readonly EnemyTargetSensor _sensor;
    private readonly EnemyFleeMovement _flee;

    public EnemyFleeState(
        EnemyTargetSensor sensor,
        EnemyFleeMovement flee)
    {
        _sensor = sensor;
        _flee = flee;
    }

    public void Enter()
    {
        _flee.Begin();
    }

    public void Tick(float deltaTime)
    {
        _flee.FleeFrom(_sensor.KnownTarget);
    }

    public void Exit()
    {
        _flee.End();
    }
}