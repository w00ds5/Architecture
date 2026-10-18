using UnityEngine;
using System;

[DefaultExecutionOrder(-50)]
[RequireComponent(
    typeof(CharacterHealth),
    typeof(CharacterActionLock))]
public sealed class CharacterAttackState : MonoBehaviour
{
    public event Action<DamageType> AttackEnded;

    private CharacterHealth _health;
    private CharacterActionLock _actionLock;

    private bool _active;
    private bool _committed;
    private DamageType _attackType;

    public bool IsAttacking => _active;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
        _actionLock = GetComponent<CharacterActionLock>();
    }

    private void OnEnable()
    {
        _health.Damaged += CancelOnDamage;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.Damaged -= CancelOnDamage;

        Cancel();
    }

    private void Update()
    {
        if (_actionLock.MovementRequested)
            Cancel();
    }

    public bool TryBegin(DamageType type)
    {
        if (!isActiveAndEnabled
            || _active
            || _health.IsDead
            || _actionLock.IsLocked
            || _actionLock.MovementRequested)
        {
            return false;
        }

        _active = true;
        _committed = false;
        _attackType = type;
        return true;
    }

    public bool TryCommit(DamageType type)
    {
        if (!_active
            || _committed
            || _attackType != type
            || _health.IsDead
            || _actionLock.MovementRequested)
        {
            return false;
        }

        _committed = true;
        return true;
    }

    public void End(DamageType type)
    {
        if (_active && _attackType == type)
            Cancel();

        AttackEnded?.Invoke(type);
    }

    private void CancelOnDamage(int amount)
    {
        Cancel();
    }

    private void Cancel()
    {
        _active = false;
        _committed = false;
    }
}