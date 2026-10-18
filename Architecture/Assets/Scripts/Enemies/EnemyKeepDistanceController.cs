using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(CharacterMovement),
    typeof(NavMeshPathfinder))]
[RequireComponent(typeof(EnemyRetreatPosition))]
public sealed class EnemyKeepDistanceController :
    MonoBehaviour,
    IEnemyMovementBehaviour
{
    [SerializeField, Min(0.1f)]
    private float minDistance = 5f;

    [SerializeField, Min(0.1f)]
    private float maxDistance = 10f;

    private EnemyTargetSensor _sensor;
    private CharacterMovement _movement;
    private NavMeshPathfinder _pathfinder;
    private EnemyRetreatPosition _retreatPosition;

    public bool AutomaticControl { get; set; } = true;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _movement = GetComponent<CharacterMovement>();
        _pathfinder = GetComponent<NavMeshPathfinder>();
        _retreatPosition = GetComponent<EnemyRetreatPosition>();
    }

    private void Update()
    {
        if (AutomaticControl)
            TickMovement(_sensor.VisibleTarget);
    }

    public void TickMovement(Transform target)
    {
        if (!isActiveAndEnabled || Time.deltaTime <= 0f)
            return;

        if (target == null)
        {
            Stop();
            return;
        }

        Vector3 toTarget =
            target.position - transform.position;

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

    public void Stop()
    {
        _pathfinder.ClearPath();
        _movement.Move(Vector3.zero, false);
    }

    private void MoveTowards(Vector3 position)
    {
        Vector3 direction =
            _pathfinder.GetDirection(position);

        _movement.Move(direction, false);
    }

    private void StopAndFace(Vector3 direction)
    {
        Stop();
        _movement.Face(direction);
    }

    private void OnValidate()
    {
        maxDistance = Mathf.Max(
            maxDistance,
            minDistance + 0.5f);
    }
}