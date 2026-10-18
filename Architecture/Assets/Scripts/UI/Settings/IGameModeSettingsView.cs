using System;

public interface IGameModeSettingsView
{
    event Action<bool> PeacefulRequested;
}