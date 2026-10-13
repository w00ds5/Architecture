using UnityEngine;
using System;

[RequireComponent(
    typeof(CharacterHealth),
    typeof(CharacterAttackState))]
public class MagicAttack : MonoBehaviour
{
    public event Action AttackStarted;

    [SerializeField] private ProjectileMovement projectilePrefab;
    [SerializeField] private LayerMask targetLayers;

    [SerializeField, Min(1)] private int damage = 30;
    [SerializeField, Min(0.1f)] private float projectileSpeed = 15f;
    [SerializeField, Min(0.01f)] private float cooldown = 0.8f;

    [SerializeField, Min(0f)] private float spawnHeight = 1.2f;
    [SerializeField, Min(0f)] private float spawnOffset = 0.8f;

    private CharacterHealth _health;
    private float _nextAttackTime;
    private CharacterAttackState _attackState;
    private Vector3 _pendingDirection;

    private void Awake()
    {
        _attackState = GetComponent<CharacterAttackState>();
        _health = GetComponent<CharacterHealth>();

        if (projectilePrefab == null
            || projectilePrefab.GetComponent<ProjectileImpact>() == null
            || projectilePrefab.GetComponent<ProjectileLifetime>() == null)
        {
            Debug.LogError(
                "Назначь префаб со всеми компонентами снаряда.",
                this);

            enabled = false;
        }
    }

    public bool TryAttack(Vector3 direction)
    {
        if (!isActiveAndEnabled
            || _health.IsDead
            || Time.time < _nextAttackTime
            || direction.sqrMagnitude < 0.001f
            || !_attackState.TryBegin(DamageType.Magical))
        {
            return false;
        }

        _pendingDirection = direction.normalized;
        _nextAttackTime = Time.time + cooldown;

        AttackStarted?.Invoke();
        return true;
    }

    public void ReleaseProjectile()
    {
        if (isActiveAndEnabled
            && _attackState.TryCommit(DamageType.Magical))
        {
            SpawnProjectile(_pendingDirection);
        }
    }

    private void SpawnProjectile(Vector3 direction)
    {
        Vector3 spawnPosition = transform.position
            + Vector3.up * spawnHeight
            + direction * spawnOffset;

        ProjectileMovement projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.LookRotation(direction));

        ProjectileImpact impact =
            projectile.GetComponent<ProjectileImpact>();

        impact.Initialize(
            new DamageInfo(damage, DamageType.Magical),
            targetLayers,
            transform);

        projectile.Launch(direction, projectileSpeed);
    }
}