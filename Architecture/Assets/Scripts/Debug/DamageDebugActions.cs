using UnityEngine;

public class DamageDebugActions : MonoBehaviour
{
    [SerializeField] private MonoBehaviour target;

    [ContextMenu("Физический урон: 10")]
    private void DealPhysicalDamage()
    {
        DealDamage(new DamageInfo(10, DamageType.Physical));
    }

    [ContextMenu("Магический урон: 20")]
    private void DealMagicalDamage()
    {
        DealDamage(new DamageInfo(20, DamageType.Magical));
    }

    private void DealDamage(DamageInfo damage)
    {
        if (!Application.isPlaying)
            return;

        if (!(target is IDamageable damageable))
        {
            Debug.LogError(
                "Target должен реализовывать IDamageable.",
                this);

            return;
        }

        damageable.TakeDamage(damage);

        Debug.Log(
            $"Отправлен урон: {damage.Type}, {damage.Amount}.",
            this);
    }
}