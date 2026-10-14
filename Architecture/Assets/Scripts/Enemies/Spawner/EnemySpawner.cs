using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private RandomSpawnArea spawnArea;
    [SerializeField] private CharacterHealth playerHealth;

    [SerializeField, Range(2, 3)] private int minCount = 2;
    [SerializeField, Range(2, 3)] private int maxCount = 3;
    [SerializeField, Min(0f)] private float playerDistance = 8f;
    [SerializeField, Min(0f)] private float spacing = 3f;

    private readonly List<Vector3> _occupiedPositions = new();

    public void SpawnRandom(IEnemyFactory factory)
    {
        if (factory == null)
            throw new System.ArgumentNullException(nameof(factory));

        if (spawnArea == null || playerHealth == null)
        {
            Debug.LogError(
                "Назначь область появления и здоровье игрока.",
                this);

            return;
        }

        _occupiedPositions.Clear();
        Physics.SyncTransforms();

        SpawnType(EnemyKind.Melee, factory);
        SpawnType(EnemyKind.Ranged, factory);
    }

    private void SpawnType(
        EnemyKind kind,
        IEnemyFactory factory)
    {
        int count = Random.Range(minCount, maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            if (!spawnArea.TryGetPoint(
                playerHealth.transform.position,
                playerDistance,
                _occupiedPositions,
                spacing,
                out Vector3 position))
            {
                Debug.LogWarning(
                    $"Не найдено свободное место для {kind}.",
                    this);

                return;
            }

            factory.Create(
                kind,
                position,
                Random.Range(0f, 360f));

            _occupiedPositions.Add(position);

            Physics.SyncTransforms();
        }
    }

    private void OnValidate()
    {
        maxCount = Mathf.Max(minCount, maxCount);
    }
}