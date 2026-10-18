using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class EnemyTargetSensor : MonoBehaviour
{
    [SerializeField] private CharacterHealth targetHealth;
    [SerializeField, Min(0f)] private float detectionRange = 8f;

    private CharacterHealth _ownHealth;

    private void Awake()
    {
        _ownHealth = GetComponent<CharacterHealth>();
    }

    public void SetTarget(CharacterHealth target)
    {
        targetHealth = target;
    }

    public Transform KnownTarget
    {
        get
        {
            if (!isActiveAndEnabled
                || _ownHealth == null
                || _ownHealth.IsDead
                || targetHealth == null
                || targetHealth.IsDead)
            {
                return null;
            }

            return targetHealth.transform;
        }
    }

    public Transform VisibleTarget
    {
        get
        {
            Transform target = KnownTarget;

            if (target == null)
                return null;

            Vector3 direction =
                target.position - transform.position;

            direction.y = 0f;

            return direction.sqrMagnitude
                <= detectionRange * detectionRange
                ? target
                : null;
        }
    }
}