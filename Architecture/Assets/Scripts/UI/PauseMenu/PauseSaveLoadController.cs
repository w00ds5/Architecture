using System;

public sealed class PauseSaveLoadController : IDisposable
{
    private readonly PauseMenuModel _model;
    private readonly IPauseMenuView _view;
    private readonly IGameSaveInteractor _saves;
    private readonly IGameStateSource _stateSource;
    private readonly IGameStartState _gameStart;
    private readonly ISceneLoader _sceneLoader;
    private readonly string _gameSceneName;
    private readonly Action<Exception> _reportError;

    public PauseSaveLoadController(
        PauseMenuModel model,
        IPauseMenuView view,
        IGameSaveInteractor saves,
        IGameStateSource stateSource,
        IGameStartState gameStart,
        ISceneLoader sceneLoader,
        string gameSceneName,
        Action<Exception> reportError)
    {
        _model = model
            ?? throw new ArgumentNullException(nameof(model));

        _view = view
            ?? throw new ArgumentNullException(nameof(view));

        _saves = saves
            ?? throw new ArgumentNullException(nameof(saves));

        _stateSource = stateSource
            ?? throw new ArgumentNullException(nameof(stateSource));

        _gameStart = gameStart
            ?? throw new ArgumentNullException(nameof(gameStart));

        _sceneLoader = sceneLoader
            ?? throw new ArgumentNullException(nameof(sceneLoader));

        _reportError = reportError
            ?? throw new ArgumentNullException(nameof(reportError));

        _gameSceneName = gameSceneName;

        _view.SaveRequested += Save;
        _view.LoadRequested += Load;
    }

    public void Dispose()
    {
        _view.SaveRequested -= Save;
        _view.LoadRequested -= Load;
    }

    private bool CanHandleRequest =>
        _model.IsOpen && !_sceneLoader.IsLoading;

    private void Save()
    {
        if (!CanHandleRequest)
            return;

        try
        {
            _saves.Save(_stateSource);
            _model.SetStatus("Игра сохранена.");
        }
        catch (Exception error)
        {
            _model.SetStatus("Не удалось сохранить игру.");
            _reportError(error);
        }
    }

    private void Load()
    {
        if (!CanHandleRequest)
            return;

        try
        {
            if (!_saves.HasSave)
            {
                _model.SetStatus("Сохранение ещё не создано.");
                return;
            }

            GameSaveData data = _saves.Load();

            _gameStart.PrepareLoad(data);
            _model.SetStatus("Загрузка…");

            _sceneLoader.Load(_gameSceneName);

            if (!_sceneLoader.IsLoading)
            {
                _gameStart.PrepareNewGame();

                _model.SetStatus(
                    "Не удалось открыть игровую сцену.");
            }
        }
        catch (Exception error)
        {
            _gameStart.PrepareNewGame();

            _model.SetStatus("Не удалось загрузить сохранение.");
            _reportError(error);
        }
    }
}