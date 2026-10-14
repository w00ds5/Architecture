using System;

public sealed class PauseMenuModel
{
    public event Action Changed;

    public bool IsOpen { get; private set; }
    public string StatusMessage { get; private set; } = string.Empty;

    public void SetOpen(bool open)
    {
        if (IsOpen == open)
            return;

        IsOpen = open;
        StatusMessage = string.Empty;

        Changed?.Invoke();
    }

    public void SetStatus(string message)
    {
        message ??= string.Empty;

        if (StatusMessage == message)
            return;

        StatusMessage = message;
        Changed?.Invoke();
    }
}