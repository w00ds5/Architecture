using UnityEngine;

public sealed class BossStrongAttackState : IState
{
    private readonly EnemyTargetSensor _sensor;
    private readonly IEnemyMovementBehaviour _movement;
    private readonly BossStrongAttack _strongAttack;

    public BossStrongAttackState(
        EnemyTargetSensor sensor,
        IEnemyMovementBehaviour movement,
        BossStrongAttack strongAttack)
    {
        _sensor = sensor;
        _movement = movement;
        _strongAttack = strongAttack;
    }

    public void Enter()
    {
    }

    public void Tick(float deltaTime)
    {
        Transform target = _sensor.VisibleTarget;

        _movement.TickMovement(target);

        if (target != null)
            _strongAttack.TryAttack();
    }

    public void Exit()
    {
    }
}