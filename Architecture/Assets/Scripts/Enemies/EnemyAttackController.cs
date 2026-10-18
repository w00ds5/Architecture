using UnityEngine;

[RequireComponent(
    typeof(EnemyTargetSensor),
    typeof(MeleeAttack),
    typeof(CharacterAttackState))]
public sealed class EnemyAttackController :
    MonoBehaviour,
    IEnemyAttackBehaviour
{
    [SerializeField, Min(0f)] private float attackRange = 1.6f;
    [SerializeField, Min(0f)] private float recoveryTime = 1.2f;

    private EnemyTargetSensor _sensor;
    private MeleeAttack _meleeAttack;
    private CharacterAttackState _attackState;
    private float _nextAttackTime;

    public bool AutomaticControl { get; set; } = true;

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
        if (AutomaticControl)
            TickAttack(_sensor.VisibleTarget);
    }

    public bool IsInRange(Transform target)
    {
        if (target == null)
            return false;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        return direction.sqrMagnitude <= attackRange * attackRange;
    }

    public void TickAttack(Transform target)
    {
        if (!isActiveAndEnabled
            || Time.deltaTime <= 0f
            || Time.time < _nextAttackTime
            || !IsInRange(target))
        {
            return;
        }

        _meleeAttack.TryAttack();
    }

    private void BeginRecovery(DamageType type)
    {
        if (type == DamageType.Physical)
            _nextAttackTime = Time.time + recoveryTime;
    }
}