using System;
using UnityEngine;

public sealed class UnityEnemyFactory : IEnemyFactory
{
    private readonly EnemySaveIdentity _meleePrefab;
    private readonly EnemySaveIdentity _rangedPrefab;
    private readonly EnemySaveIdentity _bossPrefab;

    private readonly CharacterHealth _playerHealth;
    private readonly Transform _enemyParent;
    private readonly IGameMode _gameMode;

    public UnityEnemyFactory(
        EnemySaveIdentity meleePrefab,
        EnemySaveIdentity rangedPrefab,
        EnemySaveIdentity bossPrefab,
        CharacterHealth playerHealth,
        Transform enemyParent,
        IGameMode gameMode)
    {
        if (meleePrefab == null)
            throw new ArgumentNullException(nameof(meleePrefab));

        if (rangedPrefab == null)
            throw new ArgumentNullException(nameof(rangedPrefab));

        if (bossPrefab == null)
            throw new ArgumentNullException(nameof(bossPrefab));

        if (playerHealth == null)
            throw new ArgumentNullException(nameof(playerHealth));

        if (enemyParent == null)
            throw new ArgumentNullException(nameof(enemyParent));

        if (meleePrefab.Kind != EnemyKind.Melee
            || rangedPrefab.Kind != EnemyKind.Ranged
            || bossPrefab.Kind != EnemyKind.Boss)
        {
            throw new ArgumentException(
                "Проверь Kind на префабах противников.");
        }

        _meleePrefab = meleePrefab;
        _rangedPrefab = rangedPrefab;
        _bossPrefab = bossPrefab;
        _playerHealth = playerHealth;
        _enemyParent = enemyParent;

        _gameMode = gameMode
            ?? throw new ArgumentNullException(nameof(gameMode));
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

        if (kind == EnemyKind.Boss)
        {
            BossBrain brain = enemy.GetComponent<BossBrain>();

            if (brain == null)
            {
                throw new InvalidOperationException(
                    "Добавь BossBrain на корень префаба босса.");
            }

            brain.Initialize(_gameMode);
        }
        else
        {
            EnemyBrain brain = enemy.GetComponent<EnemyBrain>();

            if (brain == null)
            {
                throw new InvalidOperationException(
                    "Добавь EnemyBrain на корень префаба моба.");
            }

            brain.Initialize(_gameMode);
        }

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

            case EnemyKind.Boss:
                return _bossPrefab;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(kind),
                    kind,
                    "Неизвестный тип противника.");
        }
    }
}