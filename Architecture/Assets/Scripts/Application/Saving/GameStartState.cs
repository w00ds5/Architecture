using System;

public sealed class GameStartState : IGameStartState
{
    private GameSaveData _pendingLoad;

    public void PrepareNewGame()
    {
        _pendingLoad = null;
    }

    public void PrepareLoad(GameSaveData data)
    {
        _pendingLoad = data
            ?? throw new ArgumentNullException(nameof(data));
    }

    public bool TryTakeLoad(out GameSaveData data)
    {
        data = _pendingLoad;
        _pendingLoad = null;

        return data != null;
    }
}