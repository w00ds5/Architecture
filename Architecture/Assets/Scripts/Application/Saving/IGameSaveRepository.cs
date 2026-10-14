public interface IGameSaveRepository
{
    bool HasSave { get; }

    void Save(GameSaveData data);

    GameSaveData Load();
}