public interface IGameSaveInteractor
{
    bool HasSave { get; }

    void Save(IGameStateSource source);

    GameSaveData Load();
}