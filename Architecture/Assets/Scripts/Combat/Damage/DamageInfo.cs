using System;

public readonly struct DamageInfo
{
    public int Amount { get; }
    public DamageType Type { get; }

    public DamageInfo(int amount, DamageType type)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Amount = amount;
        Type = type;
    }
}