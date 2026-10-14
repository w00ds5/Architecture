using System;

public sealed class MainMenuController : IDisposable
{
    private readonly MainMenuModel _model;
    private readonly IMainMenuView _view;
    private readonly ISceneLoader _sceneLoader;
    private readonly string _gameSceneName;
    private readonly IGameStartState _gameStart;

    public MainMenuController(
        MainMenuModel model,
        IMainMenuView view,
        ISceneLoader sceneLoader,
        IGameStartState gameStart,
        string gameSceneName)
    {
        _model = model;
        _view = view;
        _sceneLoader = sceneLoader;
        _gameSceneName = gameSceneName;
        _gameStart = gameStart
            ?? throw new ArgumentNullException(nameof(gameStart));

        _view.PlayRequested += Play;
        _view.SettingsRequested += OpenSettings;
        _view.BackRequested += CloseSettings;
    }

    public void Dispose()
    {
        _view.PlayRequested -= Play;
        _view.SettingsRequested -= OpenSettings;
        _view.BackRequested -= CloseSettings;
    }

    private void Play()
    {
        if (_sceneLoader.IsLoading)
            return;

        _gameStart.PrepareNewGame();
        _sceneLoader.Load(_gameSceneName);
    }

    private void OpenSettings()
    {
        _model.SetSettingsOpen(true);
    }

    private void CloseSettings()
    {
        _model.SetSettingsOpen(false);
    }
}