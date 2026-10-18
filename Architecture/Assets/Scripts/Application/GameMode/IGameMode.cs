using System;

public interface IGameMode
{
    bool IsPeaceful { get; }

    event Action Changed;

    void SetPeaceful(bool peaceful);
}