using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(
    typeof(CharacterHealth),
    typeof(CharacterAttackState))]
public class MeleeAttack : MonoBehaviour
{
    public event Action AttackStarted;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask targetLayers;

    [SerializeField, Min(1)] private int damage = 20;
    [SerializeField, Min(0.01f)] private float radius = 0.8f;
    [SerializeField, Min(0.01f)] private float cooldown = 0.6f;

    private readonly HashSet<IDamageable> _hitTargets = new();

    private CharacterHealth _health;
    private float _nextAttackTime;
    private CharacterAttackState _attackState;

    public float CooldownRemaining =>
        Mathf.Max(0f, _nextAttackTime - Time.time);

    private void Awake()
    {
        _attackState = GetComponent<CharacterAttackState>();
        _health = GetComponent<CharacterHealth>();

        if (attackPoint == null)
        {
            Debug.LogError("Назначь Attack Point.", this);
            enabled = false;
        }
    }

    public bool TryAttack()
    {
        if (!isActiveAndEnabled
            || _health.IsDead
            || Time.time < _nextAttackTime
            || !_attackState.TryBegin(DamageType.Physical))
        {
            return false;
        }

        _nextAttackTime = Time.time + cooldown;
        AttackStarted?.Invoke();
        return true;
    }

    public void ResolveHit()
    {
        if (isActiveAndEnabled
            && _attackState.TryCommit(DamageType.Physical))
        {
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        _hitTargets.Clear();

        Collider[] colliders = Physics.OverlapSphere(
            attackPoint.position,
            radius,
            targetLayers,
            QueryTriggerInteraction.Ignore);

        DamageInfo damageInfo = new DamageInfo(
            damage,
            DamageType.Physical);

        foreach (Collider hit in colliders)
        {
            if (hit.transform.IsChildOf(transform))
                continue;

            IDamageable target = hit.GetComponentInParent<IDamageable>();

            if (target != null && _hitTargets.Add(target))
                target.TakeDamage(damageInfo);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }

    public void RestoreCooldown(float remaining)
    {
        _nextAttackTime = Time.time + Mathf.Max(0f, remaining);
    }
    
}