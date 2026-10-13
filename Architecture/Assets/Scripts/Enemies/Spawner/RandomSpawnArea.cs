using System.Collections.Generic;
using UnityEngine;

public sealed class RandomSpawnArea : MonoBehaviour
{
    [SerializeField] private Vector2 size = new(40f, 40f);
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask blockingLayers;

    [SerializeField, Min(0.1f)] private float clearanceRadius = 0.6f;
    [SerializeField, Min(0.1f)] private float clearanceHeight = 2f;
    [SerializeField, Min(1)] private int maxAttempts = 100;

    public bool TryGetPoint(
        Vector3 playerPosition,
        float playerDistance,
        IReadOnlyList<Vector3> occupiedPositions,
        float spacing,
        out Vector3 point)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidate = transform.position + new Vector3(
                Random.Range(-size.x * 0.5f, size.x * 0.5f),
                0f,
                Random.Range(-size.y * 0.5f, size.y * 0.5f));

            if (!Physics.Raycast(
                candidate + Vector3.up * 20f,
                Vector3.down,
                out RaycastHit hit,
                40f,
                groundLayers,
                QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            candidate = hit.point + Vector3.up * 0.05f;

            if (HorizontalDistanceSquared(candidate, playerPosition)
                < playerDistance * playerDistance)
            {
                continue;
            }

            bool tooClose = false;

            foreach (Vector3 occupied in occupiedPositions)
            {
                if (HorizontalDistanceSquared(candidate, occupied)
                    < spacing * spacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            Vector3 bottom =
                candidate + Vector3.up * clearanceRadius;

            Vector3 top =
                candidate + Vector3.up
                * (clearanceHeight - clearanceRadius);

            if (Physics.CheckCapsule(
                bottom,
                top,
                clearanceRadius,
                blockingLayers,
                QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            point = candidate;
            return true;
        }

        point = default;
        return false;
    }

    private static float HorizontalDistanceSquared(Vector3 a, Vector3 b)
    {
        Vector3 difference = a - b;
        difference.y = 0f;
        return difference.sqrMagnitude;
    }

    private void OnValidate()
    {
        size.x = Mathf.Max(1f, size.x);
        size.y = Mathf.Max(1f, size.y);
        clearanceHeight = Mathf.Max(
            clearanceHeight, clearanceRadius * 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(size.x, 0.1f, size.y));
    }
}