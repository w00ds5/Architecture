using UnityEngine;

[RequireComponent(
    typeof(CharacterHealth),
    typeof(MeleeAttack),
    typeof(CharacterAttackState))]
public sealed class BossStrongAttack : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int damage = 40;

    [SerializeField, Min(0.1f)]
    private float cooldown = 8f;

    [SerializeField, Min(0f)]
    private float recoveryTime = 1.5f;

    private CharacterHealth _health;
    private MeleeAttack _meleeAttack;
    private CharacterAttackState _attackState;

    private float _nextAttackTime;
    private float _recoveryUntil;

    public bool IsExecuting { get; private set; }

    public float CooldownRemaining =>
        Mathf.Max(0f, _nextAttackTime - Time.time);

    public bool CanAttack =>
        isActiveAndEnabled
        && !_health.IsDead
        && !_attackState.IsAttacking
        && Time.time >= _nextAttackTime
        && Time.time >= _recoveryUntil;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
        _meleeAttack = GetComponent<MeleeAttack>();
        _attackState = GetComponent<CharacterAttackState>();
    }

    private void OnEnable()
    {
        _attackState.AttackEnded += OnAttackEnded;
        _health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        if (_attackState != null)
            _attackState.AttackEnded -= OnAttackEnded;

        if (_health != null)
            _health.Damaged -= OnDamaged;

        IsExecuting = false;
    }

    public bool TryAttack()
    {
        if (!CanAttack || !_meleeAttack.TryAttack(damage))
            return false;

        IsExecuting = true;
        _nextAttackTime = Time.time + cooldown;

        return true;
    }

    public void RestoreCooldown(float remaining)
    {
        _nextAttackTime = Time.time + Mathf.Max(0f, remaining);
        IsExecuting = false;
        _recoveryUntil = 0f;
    }

    private void OnAttackEnded(DamageType type)
    {
        if (type != DamageType.Physical)
            return;

        IsExecuting = false;
        _recoveryUntil = Time.time + recoveryTime;
    }

    private void OnDamaged(int amount)
    {
        IsExecuting = false;
    }
}