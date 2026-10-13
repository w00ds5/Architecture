using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(CharacterMovement),
    typeof(NavMeshPathfinder))]
public class EnemyChaseController : MonoBehaviour
{
    [SerializeField, Min(0f)]
    private float stopDistance = 1.6f;

    private EnemyTargetSensor _sensor;
    private CharacterMovement _movement;
    private NavMeshPathfinder _pathfinder;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _movement = GetComponent<CharacterMovement>();
        _pathfinder = GetComponent<NavMeshPathfinder>();
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
            _pathfinder.ClearPath();
            _movement.Move(Vector3.zero, false);
            _movement.Face(toTarget);
        }
    }
}