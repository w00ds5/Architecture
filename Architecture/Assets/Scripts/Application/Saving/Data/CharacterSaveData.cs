using System;

[Serializable]
public sealed class CharacterSaveData
{
    public float positionX;
    public float positionY;
    public float positionZ;

    public float rotationY;

    public int health;
    public float mana;

    public float meleeCooldown;
    public float magicCooldown;
}