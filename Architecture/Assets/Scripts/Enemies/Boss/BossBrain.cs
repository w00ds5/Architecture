using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
    typeof(CharacterHealth),
    typeof(EnemyTargetSensor),
    typeof(BossProvocation))]
[RequireComponent(typeof(BossStrongAttack))]
public sealed class BossBrain : MonoBehaviour
{
    [SerializeField]
    private BossStateId currentState;

    private readonly StateMachine _machine = new();

    private readonly Dictionary<BossStateId, IState> _states = new();

    private CharacterHealth _health;
    private EnemyTargetSensor _sensor;
    private BossProvocation _provocation;
    private BossStrongAttack _strongAttack;
    private IEnemyMovementBehaviour _movement;
    private IEnemyAttackBehaviour _attack;
    private BossStateSelector _selector;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
        _sensor = GetComponent<EnemyTargetSensor>();
        _provocation = GetComponent<BossProvocation>();
        _strongAttack = GetComponent<BossStrongAttack>();

        _movement = GetComponent<IEnemyMovementBehaviour>();
        _attack = GetComponent<IEnemyAttackBehaviour>();
    }

    public void Initialize(IGameMode mode)
    {
        if (_selector != null)
            return;

        if (mode == null)
            throw new ArgumentNullException(nameof(mode));

        if (_movement == null || _attack == null)
        {
            throw new InvalidOperationException(
                "Назначь боссу компоненты движения и атаки, "
                + "реализующие интерфейсы поведения.");
        }

        _movement.AutomaticControl = false;
        _attack.AutomaticControl = false;

        _states.Add(
            BossStateId.Idle,
            new EnemyIdleState(_movement));

        _states.Add(
            BossStateId.Aggression,
            new EnemyAggressionState(_sensor, _movement));

        _states.Add(
            BossStateId.Attack,
            new EnemyAttackState(_sensor, _movement, _attack));

        _states.Add(
            BossStateId.StrongAttack,
            new BossStrongAttackState(
                _sensor,
                _movement,
                _strongAttack));

        _selector = new BossStateSelector(
            _sensor,
            _attack,
            _strongAttack,
            _provocation,
            mode);

        currentState = BossStateId.Idle;
        _machine.ChangeState(_states[currentState]);
    }

    private void Update()
    {
        if (_selector == null || Time.deltaTime <= 0f)
            return;

        if (_health.IsDead)
        {
            _machine.Clear();
            _movement.Stop();
            return;
        }

        currentState = _selector.Select();

        _machine.ChangeState(_states[currentState]);
        _machine.Tick(Time.deltaTime);
    }

    private void OnDisable()
    {
        _machine.Clear();
    }
}