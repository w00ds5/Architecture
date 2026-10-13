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

        if (targetHealth == null)
        {
            Debug.LogError("Назначь Target Health.", this);
            enabled = false;
        }
    }

    public Transform VisibleTarget
    {
        get
        {
            if (!isActiveAndEnabled
                || _ownHealth.IsDead
                || targetHealth.IsDead)
            {
                return null;
            }

            Vector3 toTarget =
                targetHealth.transform.position - transform.position;

            toTarget.y = 0f;

            return toTarget.sqrMagnitude <= detectionRange * detectionRange
                ? targetHealth.transform
                : null;
        }
    }
}