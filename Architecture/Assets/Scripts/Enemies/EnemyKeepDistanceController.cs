using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(CharacterMovement),
    typeof(NavMeshPathfinder))]
[RequireComponent(typeof(EnemyRetreatPosition))]
public sealed class EnemyKeepDistanceController : MonoBehaviour
{
    [SerializeField, Min(0.1f)]
    private float minDistance = 5f;

    [SerializeField, Min(0.1f)]
    private float maxDistance = 10f;

    private EnemyTargetSensor _sensor;
    private CharacterMovement _movement;
    private NavMeshPathfinder _pathfinder;
    private EnemyRetreatPosition _retreatPosition;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _movement = GetComponent<CharacterMovement>();
        _pathfinder = GetComponent<NavMeshPathfinder>();
        _retreatPosition = GetComponent<EnemyRetreatPosition>();
    }

    private void Update()
    {
        Transform target = _sensor.VisibleTarget;

        if (target == null)
        {
            _pathfinder.ClearPath();
            _movement.Move(Vector3.zero, false);
            return;
        }

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        float distanceSquared = toTarget.sqrMagnitude;

        if (distanceSquared > maxDistance * maxDistance)
        {
            MoveTowards(target.position);
        }
        else if (distanceSquared < minDistance * minDistance)
        {
            if (_retreatPosition.TryGetPosition(
                    target.position,
                    minDistance,
                    maxDistance,
                    out Vector3 position))
            {
                MoveTowards(position);
            }
            else
            {
                StopAndFace(toTarget);
            }
        }
        else
        {
            StopAndFace(toTarget);
        }
    }

    private void MoveTowards(Vector3 position)
    {
        Vector3 direction = _pathfinder.GetDirection(position);
        _movement.Move(direction, false);
    }

    private void StopAndFace(Vector3 direction)
    {
        _pathfinder.ClearPath();
        _movement.Move(Vector3.zero, false);
        _movement.Face(direction);
    }

    private void OnValidate()
    {
        maxDistance = Mathf.Max(maxDistance, minDistance + 0.5f);
    }
}