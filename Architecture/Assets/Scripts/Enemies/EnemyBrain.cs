using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
    typeof(CharacterHealth),
    typeof(EnemyTargetSensor),
    typeof(EnemyFleeMovement))]
public sealed class EnemyBrain : MonoBehaviour
{
    [SerializeField, Range(0.01f, 0.99f)]
    private float fleeHealthThreshold = 0.3f;

    [SerializeField]
    private EnemyStateId currentState;

    private readonly StateMachine _machine = new();

    private readonly Dictionary<EnemyStateId, IState> _states = new();

    private CharacterHealth _health;
    private EnemyTargetSensor _sensor;
    private EnemyFleeMovement _flee;
    private IEnemyMovementBehaviour _movement;
    private IEnemyAttackBehaviour _attack;
    private EnemyStateSelector _selector;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
        _sensor = GetComponent<EnemyTargetSensor>();
        _flee = GetComponent<EnemyFleeMovement>();

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
                "На мобе нужны компоненты движения и атаки, "
                + "реализующие интерфейсы поведения.");
        }

        _movement.AutomaticControl = false;
        _attack.AutomaticControl = false;

        _states.Add(
            EnemyStateId.Idle,
            new EnemyIdleState(_movement));

        _states.Add(
            EnemyStateId.Aggression,
            new EnemyAggressionState(_sensor, _movement));

        _states.Add(
            EnemyStateId.Attack,
            new EnemyAttackState(_sensor, _movement, _attack));

        _states.Add(
            EnemyStateId.Flee,
            new EnemyFleeState(_sensor, _flee));

        _selector = new EnemyStateSelector(
            _health,
            _sensor,
            _attack,
            _flee,
            mode,
            fleeHealthThreshold);

        currentState = EnemyStateId.Idle;
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