using UnityEngine;

public sealed class BossStateSelector
{
    private readonly EnemyTargetSensor _sensor;
    private readonly IEnemyAttackBehaviour _attack;
    private readonly BossStrongAttack _strongAttack;
    private readonly BossProvocation _provocation;
    private readonly IGameMode _mode;

    public BossStateSelector(
        EnemyTargetSensor sensor,
        IEnemyAttackBehaviour attack,
        BossStrongAttack strongAttack,
        BossProvocation provocation,
        IGameMode mode)
    {
        _sensor = sensor;
        _attack = attack;
        _strongAttack = strongAttack;
        _provocation = provocation;
        _mode = mode;
    }

    public BossStateId Select()
    {
        Transform target = _sensor.VisibleTarget;

        if (target == null)
            return BossStateId.Idle;

        if (_mode.IsPeaceful && !_provocation.IsProvoked)
            return BossStateId.Idle;

        if (_strongAttack.IsExecuting)
            return BossStateId.StrongAttack;

        if (!_attack.IsInRange(target))
            return BossStateId.Aggression;

        return _strongAttack.CanAttack
            ? BossStateId.StrongAttack
            : BossStateId.Attack;
    }
}