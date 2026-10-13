using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(MeleeAttack),
    typeof(CharacterAttackState))]
public sealed class EnemyAttackController : MonoBehaviour
{
    [SerializeField, Min(0f)] private float attackRange = 1.6f;
    [SerializeField, Min(0f)] private float recoveryTime = 1.2f;

    private EnemyTargetSensor _sensor;
    private MeleeAttack _meleeAttack;
    private CharacterAttackState _attackState;
    private float _nextAttackTime;

    private void Awake()
    {
        _sensor = GetComponent<EnemyTargetSensor>();
        _meleeAttack = GetComponent<MeleeAttack>();
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

        if (direction.sqrMagnitude <= attackRange * attackRange)
            _meleeAttack.TryAttack();
    }

    private void BeginRecovery(DamageType type)
    {
        if (type == DamageType.Physical)
            _nextAttackTime = Time.time + recoveryTime;
    }
}