using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(MagicAttack),
    typeof(CharacterAttackState))]
public sealed class EnemyRangedAttackController : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float minAttackDistance = 5f;
    [SerializeField, Min(0.1f)] private float maxAttackDistance = 10f;
    [SerializeField, Min(0f)] private float recoveryTime = 1.2f;

    private EnemyTargetSensor _sensor;
    private MagicAttack _magicAttack;
    private CharacterAttackState _attackState;
    private float _nextAttackTime;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _magicAttack = GetComponent<MagicAttack>();
        _attackState = GetComponent<CharacterAttackState>();
    }

    private void OnEnable()
    {
        _attackState.AttackEnded += BeginRecovery;
    }

    private void OnDisable()
    {
        if (_attackState != null)
            _attackState.AttackEnded -= BeginRecovery;
    }

    private void Update()
    {
        if (Time.time < _nextAttackTime)
            return;

        Transform target = _sensor.VisibleTarget;
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        float distanceSquared = direction.sqrMagnitude;

        if (distanceSquared < minAttackDistance * minAttackDistance
            || distanceSquared > maxAttackDistance * maxAttackDistance)
        {
            return;
        }

        if (Vector3.Angle(transform.forward, direction) > 10f)
            return;

        _magicAttack.TryAttack(direction);
    }

    private void BeginRecovery(DamageType type)
    {
        if (type == DamageType.Magical)
            _nextAttackTime = Time.time + recoveryTime;
    }
}