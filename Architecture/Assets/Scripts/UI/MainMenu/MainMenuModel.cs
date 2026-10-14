using System;

public sealed class MainMenuModel
{
    public event Action Changed;

    public bool SettingsOpen { get; private set; }

    public void SetSettingsOpen(bool open)
    {
        if (SettingsOpen == open)
            return;

        SettingsOpen = open;
        Changed?.Invoke();
    }
}