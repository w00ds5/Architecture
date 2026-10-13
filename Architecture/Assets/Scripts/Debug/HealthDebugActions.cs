using UnityEngine;

public class HealthDebugActions : MonoBehaviour
{
    [SerializeField] private CharacterHealth health;

    private void OnEnable()
    {
        if (health != null)
            health.Died += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null)
            health.Died -= HandleDeath;
    }

    [ContextMenu("Нанести 10 урона")]
    private void DealTestDamage()
    {
        if (!Application.isPlaying || health == null)
            return;

        health.TakeDamage(10);

        Debug.Log(
            $"HP: {health.CurrentHealth}/{health.MaxHealth}",
            this);
    }

    private void HandleDeath()
    {
        Debug.Log("Получено событие смерти.", this);
    }
}