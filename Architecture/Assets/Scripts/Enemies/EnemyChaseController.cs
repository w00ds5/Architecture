using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(CharacterMovement),
    typeof(NavMeshPathfinder))]
public class EnemyChaseController :
    MonoBehaviour,
    IEnemyMovementBehaviour
{
    [SerializeField, Min(0f)]
    private float stopDistance = 1.6f;

    private EnemyTargetSensor _sensor;
    private CharacterMovement _movement;
    private NavMeshPathfinder _pathfinder;

    public bool AutomaticControl { get; set; } = true;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _movement = GetComponent<CharacterMovement>();
        _pathfinder = GetComponent<NavMeshPathfinder>();
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

        if (toTarget.sqrMagnitude
            > stopDistance * stopDistance)
        {
            Vector3 direction =
                _pathfinder.GetDirection(target.position);

            _movement.Move(direction, false);
        }
        else
        {
            Stop();
            _movement.Face(toTarget);
        }
    }

    public void Stop()
    {
        _pathfinder.ClearPath();
        _movement.Move(Vector3.zero, false);
    }
}