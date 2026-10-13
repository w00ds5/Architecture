using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(CharacterMovement))]
public class EnemyChaseController : MonoBehaviour
{
    [SerializeField, Min(0f)] private float stopDistance = 1.6f;

    private EnemyTargetSensor _sensor;
    private CharacterMovement _movement;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _movement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        Transform target = _sensor.VisibleTarget;

        if (target == null)
        {
            _movement.Move(Vector3.zero, false);
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > stopDistance * stopDistance)
        {
            _movement.Move(direction, false);
        }
        else
        {
            _movement.Move(Vector3.zero, false);
            _movement.Face(direction);
        }
    }
}