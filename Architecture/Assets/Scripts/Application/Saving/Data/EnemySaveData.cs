using System;

[Serializable]
public sealed class EnemySaveData
{
    public EnemyKind kind;
    public CharacterSaveData state;
}