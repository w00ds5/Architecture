using System;

public sealed class ManaModel
{
    public float MaxMana { get; }
    public float CurrentMana { get; private set; }

    public event Action<float, float> Changed;

    public ManaModel(float maxMana)
    {
        if (maxMana <= 0f)
            throw new ArgumentOutOfRangeException(nameof(maxMana));

        MaxMana = maxMana;
        CurrentMana = maxMana;
    }

    public bool CanSpend(float amount)
    {
        return amount >= 0f && CurrentMana >= amount;
    }

    public bool TrySpend(float amount)
    {
        if (!CanSpend(amount))
            return false;

        SetCurrent(CurrentMana - amount);
        return true;
    }

    public void Restore(float amount)
    {
        if (amount > 0f)
            SetCurrent(CurrentMana + amount);
    }

    public void SetCurrent(float value)
    {
        float clamped = Math.Max(0f, Math.Min(MaxMana, value));

        if (CurrentMana == clamped)
            return;

        CurrentMana = clamped;
        Changed?.Invoke(CurrentMana, MaxMana);
    }
}