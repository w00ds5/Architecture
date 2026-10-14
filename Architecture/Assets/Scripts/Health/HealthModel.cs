using System;

public sealed class HealthModel
{
    public int MaxHealth { get; }
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth == 0;

    public event Action<int, int> Changed;
    public event Action<int> Damaged;
    public event Action Died;

    public HealthModel(int maxHealth)
    {
        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth));

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || IsDead)
            return;

        int appliedDamage = Math.Min(amount, CurrentHealth);
        CurrentHealth -= appliedDamage;

        Changed?.Invoke(CurrentHealth, MaxHealth);
        Damaged?.Invoke(appliedDamage);

        if (IsDead)
            Died?.Invoke();
    }

    public void RestoreState(int currentHealth)
    {
        int restoredHealth = Math.Max(
            0,
            Math.Min(currentHealth, MaxHealth));

        if (CurrentHealth == restoredHealth)
            return;

        bool wasDead = IsDead;

        CurrentHealth = restoredHealth;
        Changed?.Invoke(CurrentHealth, MaxHealth);

        if (!wasDead && IsDead)
            Died?.Invoke();
    }
}