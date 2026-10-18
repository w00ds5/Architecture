using System;

public sealed class GameModeSettingsController : IDisposable
{
    private readonly IGameModeSettingsView _view;
    private readonly IGameMode _mode;

    public GameModeSettingsController(
        IGameModeSettingsView view,
        IGameMode mode)
    {
        _view = view
            ?? throw new ArgumentNullException(nameof(view));

        _mode = mode
            ?? throw new ArgumentNullException(nameof(mode));

        _view.PeacefulRequested += ChangeMode;
    }

    public void Dispose()
    {
        _view.PeacefulRequested -= ChangeMode;
    }

    private void ChangeMode(bool peaceful)
    {
        _mode.SetPeaceful(peaceful);
    }
}