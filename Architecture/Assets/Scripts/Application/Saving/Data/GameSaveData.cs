using System;

[Serializable]
public sealed class GameSaveData
{
    public const int CurrentVersion = 1;

    public int version = CurrentVersion;

    public CharacterSaveData player;

    public EnemySaveData[] enemies =
        Array.Empty<EnemySaveData>();
}