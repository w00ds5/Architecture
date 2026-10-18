using UnityEngine;

public sealed class EnemyAttackState : IState
{
    private readonly EnemyTargetSensor _sensor;
    private readonly IEnemyMovementBehaviour _movement;
    private readonly IEnemyAttackBehaviour _attack;

    public EnemyAttackState(
        EnemyTargetSensor sensor,
        IEnemyMovementBehaviour movement,
        IEnemyAttackBehaviour attack)
    {
        _sensor = sensor;
        _movement = movement;
        _attack = attack;
    }

    public void Enter()
    {
    }

    public void Tick(float deltaTime)
    {
        Transform target = _sensor.VisibleTarget;

        _movement.TickMovement(target);
        _attack.TickAttack(target);
    }

    public void Exit()
    {
    }
}