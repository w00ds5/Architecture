public interface ICharacterSaveState
{
    bool IsAlive { get; }

    CharacterSaveData Capture();
    void Restore(CharacterSaveData data);
}