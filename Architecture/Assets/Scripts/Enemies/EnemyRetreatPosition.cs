using UnityEngine;

[RequireComponent(typeof(NavMeshPathfinder))]
public sealed class EnemyRetreatPosition : MonoBehaviour
{
    [SerializeField, Min(0.1f)]
    private float searchInterval = 0.4f;

    private static readonly float[] SearchAngles =
    {
        0f, 45f, -45f, 90f, -90f, 135f, -135f, 180f
    };

    private NavMeshPathfinder _pathfinder;
    private float _nextSearchTime;
    private Vector3 _position;
    private bool _hasPosition;

    private void Awake()
    {
        _pathfinder = GetComponent<NavMeshPathfinder>();
    }

    public bool TryGetPosition(
        Vector3 targetPosition,
        float minDistance,
        float maxDistance,
        out Vector3 position)
    {
        if (Time.time >= _nextSearchTime)
        {
            _nextSearchTime = Time.time + searchInterval;

            _hasPosition = FindPosition(
                targetPosition,
                minDistance,
                maxDistance,
                out _position);
        }

        position = _position;
        return _hasPosition;
    }

    private bool FindPosition(
        Vector3 targetPosition,
        float minDistance,
        float maxDistance,
        out Vector3 position)
    {
        position = Vector3.zero;

        Vector3 away = transform.position - targetPosition;
        away.y = 0f;

        away = away.sqrMagnitude > 0.001f
            ? away.normalized
            : -transform.forward;

        float desiredDistance =
            (minDistance + maxDistance) * 0.5f;

        foreach (float angle in SearchAngles)
        {
            Vector3 direction =
                Quaternion.Euler(0f, angle, 0f) * away;

            Vector3 candidate =
                targetPosition + direction * desiredDistance;

            if (!_pathfinder.TryFindReachablePoint(
                    candidate,
                    out Vector3 point))
            {
                continue;
            }

            Vector3 fromTarget = point - targetPosition;
            fromTarget.y = 0f;

            float distanceSquared = fromTarget.sqrMagnitude;

            if (distanceSquared < minDistance * minDistance
                || distanceSquared > maxDistance * maxDistance)
            {
                continue;
            }

            position = point;
            return true;
        }

        return false;
    }
}