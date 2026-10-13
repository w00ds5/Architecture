using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class DamageReceiver : MonoBehaviour, IDamageable
{
    private CharacterHealth _health;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (!isActiveAndEnabled)
            return;

        _health.TakeDamage(damage.Amount);
    }
}