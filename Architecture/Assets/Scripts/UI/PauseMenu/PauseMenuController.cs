using System;

public sealed class PauseMenuController : IDisposable
{
    private readonly PauseMenuModel _model;
    private readonly IPauseMenuView _view;
    private readonly IPauseMenuInput _input;
    private readonly IGameplayPauseService _pauseService;
    private readonly ISceneLoader _sceneLoader;
    private readonly string _mainMenuSceneName;

    public PauseMenuController(
        PauseMenuModel model,
        IPauseMenuView view,
        IPauseMenuInput input,
        IGameplayPauseService pauseService,
        ISceneLoader sceneLoader,
        string mainMenuSceneName)
    {
        _model = model;
        _view = view;
        _input = input;
        _pauseService = pauseService;
        _sceneLoader = sceneLoader;
        _mainMenuSceneName = mainMenuSceneName;

        _view.ResumeRequested += Resume;
        _view.MainMenuRequested += ReturnToMainMenu;
        _model.Changed += ApplyPause;
    }

    public void Tick()
    {
        if (_sceneLoader.IsLoading)
            return;

        if (!_input.PausePressed)
            return;

        if (!_model.IsOpen && !_pauseService.CanPause)
            return;

        _model.SetOpen(!_model.IsOpen);
    }

    public void Dispose()
    {
        _view.ResumeRequested -= Resume;
        _view.MainMenuRequested -= ReturnToMainMenu;
        _model.Changed -= ApplyPause;

        _pauseService.SetPaused(false);
    }

    private void Resume()
    {
        if (_sceneLoader.IsLoading)
            return;

        _model.SetOpen(false);
    }

    private void ReturnToMainMenu()
    {
        if (_sceneLoader.IsLoading)
            return;

        Resume();
        _sceneLoader.Load(_mainMenuSceneName);
    }

    private void ApplyPause()
    {
        _pauseService.SetPaused(_model.IsOpen);
    }
}