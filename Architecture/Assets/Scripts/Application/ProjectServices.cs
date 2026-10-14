using System;

public sealed class ProjectServices
{
    public ISceneLoader SceneLoader { get; }
    public IMusicService Music { get; }
    public IGameSaveInteractor Saves { get; }
    public IGameStartState GameStart { get; }

    public ProjectServices(
        ISceneLoader sceneLoader,
        IMusicService music,
        IGameSaveInteractor saves,
        IGameStartState gameStart)
    {
        SceneLoader = sceneLoader
            ?? throw new ArgumentNullException(nameof(sceneLoader));

        Music = music
            ?? throw new ArgumentNullException(nameof(music));

        Saves = saves
            ?? throw new ArgumentNullException(nameof(saves));

        GameStart = gameStart
            ?? throw new ArgumentNullException(nameof(gameStart));
    }
}