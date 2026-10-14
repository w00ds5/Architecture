public interface IGameStartState
{
    void PrepareNewGame();

    void PrepareLoad(GameSaveData data);

    bool TryTakeLoad(out GameSaveData data);
}