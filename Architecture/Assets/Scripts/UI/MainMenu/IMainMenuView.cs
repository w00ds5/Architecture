using System;

public interface IMainMenuView
{
    event Action PlayRequested;
    event Action SettingsRequested;
    event Action BackRequested;
}