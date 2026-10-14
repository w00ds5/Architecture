using System;
using UnityEngine;

public sealed class UnityEnemyFactory : IEnemyFactory
{
    private readonly EnemySaveIdentity _meleePrefab;
    private readonly EnemySaveIdentity _rangedPrefab;
    private readonly CharacterHealth _playerHealth;
    private readonly Transform _enemyParent;

    public UnityEnemyFactory(
        EnemySaveIdentity meleePrefab,
        EnemySaveIdentity rangedPrefab,
        CharacterHealth playerHealth,
        Transform enemyParent)
    {
        if (meleePrefab == null)
            throw new ArgumentNullException(nameof(meleePrefab));

        if (rangedPrefab == null)
            throw new ArgumentNullException(nameof(rangedPrefab));

        if (playerHealth == null)
            throw new ArgumentNullException(nameof(playerHealth));

        if (enemyParent == null)
            throw new ArgumentNullException(nameof(enemyParent));

        if (meleePrefab.Kind != EnemyKind.Melee
            || rangedPrefab.Kind != EnemyKind.Ranged)
        {
            throw new ArgumentException(
                "Проверь Kind на префабах противников.");
        }

        _meleePrefab = meleePrefab;
        _rangedPrefab = rangedPrefab;
        _playerHealth = playerHealth;
        _enemyParent = enemyParent;
    }

    public ICharacterSaveState Create(
        EnemyKind kind,
        Vector3 position,
        float rotationY)
    {
        EnemySaveIdentity prefab = GetPrefab(kind);

        EnemySaveIdentity enemy = UnityEngine.Object.Instantiate(
            prefab,
            position,
            Quaternion.Euler(0f, rotationY, 0f),
            _enemyParent);

        enemy.GetComponent<EnemyTargetSensor>()
            .SetTarget(_playerHealth);

        return enemy.State;
    }

    private EnemySaveIdentity GetPrefab(EnemyKind kind)
    {
        switch (kind)
        {
            case EnemyKind.Melee:
                return _meleePrefab;

            case EnemyKind.Ranged:
                return _rangedPrefab;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(kind),
                    kind,
                    "Неизвестный тип противника.");
        }
    }
}