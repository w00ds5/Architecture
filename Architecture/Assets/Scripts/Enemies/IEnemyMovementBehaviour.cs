using UnityEngine;

public interface IEnemyMovementBehaviour
{
    bool AutomaticControl { get; set; }

    void TickMovement(Transform target);

    void Stop();
}