using System;

public sealed class GameMode : IGameMode
{
    public bool IsPeaceful { get; private set; }

    public event Action Changed;

    public void SetPeaceful(bool peaceful)
    {
        if (IsPeaceful == peaceful)
            return;

        IsPeaceful = peaceful;
        Changed?.Invoke();
    }
}