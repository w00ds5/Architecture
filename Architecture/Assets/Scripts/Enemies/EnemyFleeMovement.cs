using UnityEngine;

[RequireComponent(
    typeof(CharacterMovement),
    typeof(NavMeshPathfinder),
    typeof(EnemyRetreatPosition))]
public sealed class EnemyFleeMovement : MonoBehaviour
{
    [SerializeField, Min(1f)]
    private float safeDistance = 18f;

    [SerializeField, Min(0.5f)]
    private float searchMargin = 4f;

    private CharacterMovement _movement;
    private NavMeshPathfinder _pathfinder;
    private EnemyRetreatPosition _retreatPosition;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _pathfinder = GetComponent<NavMeshPathfinder>();
        _retreatPosition = GetComponent<EnemyRetreatPosition>();
    }

    public bool IsSafeFrom(Transform threat)
    {
        if (threat == null)
            return true;

        Vector3 direction =
            transform.position - threat.position;

        direction.y = 0f;

        return direction.sqrMagnitude
            >= safeDistance * safeDistance;
    }

    public void Begin()
    {
        _pathfinder.ClearPath();
        _retreatPosition.ResetSearch();
    }

    public void FleeFrom(Transform threat)
    {
        if (threat == null || IsSafeFrom(threat))
        {
            Stop();
            return;
        }

        if (!_retreatPosition.TryGetPosition(
            threat.position,
            safeDistance,
            safeDistance + searchMargin,
            out Vector3 destination))
        {
            Stop();
            return;
        }

        Vector3 direction =
            _pathfinder.GetDirection(destination);

        _movement.Move(direction, true);
    }

    public void Stop()
    {
        _pathfinder.ClearPath();
        _movement.Move(Vector3.zero, false);
    }

    public void End()
    {
        _pathfinder.ClearPath();
        _retreatPosition.ResetSearch();
    }
}