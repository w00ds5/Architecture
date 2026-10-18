using UnityEngine;

public interface IEnemyAttackBehaviour
{
    bool AutomaticControl { get; set; }

    bool IsInRange(Transform target);

    void TickAttack(Transform target);
}