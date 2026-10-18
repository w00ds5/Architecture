using UnityEngine;

public sealed class EnemyStateSelector
{
    private readonly CharacterHealth _health;
    private readonly EnemyTargetSensor _sensor;
    private readonly IEnemyAttackBehaviour _attack;
    private readonly EnemyFleeMovement _flee;
    private readonly IGameMode _mode;
    private readonly float _fleeHealthThreshold;

    public EnemyStateSelector(
        CharacterHealth health,
        EnemyTargetSensor sensor,
        IEnemyAttackBehaviour attack,
        EnemyFleeMovement flee,
        IGameMode mode,
        float fleeHealthThreshold)
    {
        _health = health;
        _sensor = sensor;
        _attack = attack;
        _flee = flee;
        _mode = mode;
        _fleeHealthThreshold = Mathf.Clamp01(fleeHealthThreshold);
    }

    public EnemyStateId Select()
    {
        Transform knownTarget = _sensor.KnownTarget;

        if (knownTarget == null)
            return EnemyStateId.Idle;

        float healthFraction =
            (float)_health.CurrentHealth / _health.MaxHealth;

        if (healthFraction <= _fleeHealthThreshold
            && !_flee.IsSafeFrom(knownTarget))
        {
            return EnemyStateId.Flee;
        }

        if (_mode.IsPeaceful)
            return EnemyStateId.Idle;

        Transform visibleTarget = _sensor.VisibleTarget;

        if (visibleTarget == null)
            return EnemyStateId.Idle;

        return _attack.IsInRange(visibleTarget)
            ? EnemyStateId.Attack
            : EnemyStateId.Aggression;
    }
}