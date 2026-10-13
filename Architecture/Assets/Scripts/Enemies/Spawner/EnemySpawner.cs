using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private RandomSpawnArea spawnArea;
    [SerializeField] private CharacterHealth playerHealth;
    [SerializeField] private EnemyTargetSensor meleePrefab;
    [SerializeField] private EnemyTargetSensor rangedPrefab;
    [SerializeField] private Transform enemyParent;

    [SerializeField, Range(2, 3)] private int minCount = 2;
    [SerializeField, Range(2, 3)] private int maxCount = 3;
    [SerializeField, Min(0f)] private float playerDistance = 8f;
    [SerializeField, Min(0f)] private float spacing = 3f;

    private readonly List<Vector3> _occupiedPositions = new();

    private void Start()
    {
        if (spawnArea == null
            || playerHealth == null
            || meleePrefab == null
            || rangedPrefab == null)
        {
            Debug.LogError("Назначь область, игрока и оба префаба.", this);
            enabled = false;
            return;
        }

        Physics.SyncTransforms();

        SpawnType(meleePrefab);
        SpawnType(rangedPrefab);
    }

    private void SpawnType(EnemyTargetSensor prefab)
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
                    $"Не найдено свободное место для {prefab.name}.",
                    this);
                return;
            }

            Transform parent = enemyParent != null
                ? enemyParent
                : transform;

            EnemyTargetSensor enemy = Instantiate(
                prefab,
                position,
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
                parent);

            enemy.SetTarget(playerHealth);
            _occupiedPositions.Add(position);
        }
    }

    private void OnValidate()
    {
        maxCount = Mathf.Max(minCount, maxCount);
    }
}