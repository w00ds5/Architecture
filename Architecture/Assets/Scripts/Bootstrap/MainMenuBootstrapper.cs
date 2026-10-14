using UnityEngine;

public sealed class MainMenuBootstrapper :
    MonoBehaviour,
    ISceneBootstrapper
{
    [SerializeField] private MainMenuView menuView;
    [SerializeField] private MusicSettingsView musicSettingsView;
    [SerializeField] private string gameSceneName = "Game";

    private MainMenuController _controller;
    private MusicSettingsController _musicSettingsController;

    public void Initialize(ProjectServices services)
    {
        if (_controller != null)
            return;

        if (menuView == null || musicSettingsView == null)
        {
            Debug.LogError(
                "Назначь Menu View и Music Settings View.",
                this);

            return;
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        MainMenuModel menuModel = new MainMenuModel();
        menuView.Initialize(menuModel);

        MusicSettingsModel musicModel =
            new MusicSettingsModel(services.Music.Volume);

        musicSettingsView.Initialize(musicModel);

        _musicSettingsController = new MusicSettingsController(
            musicModel,
            musicSettingsView,
            services.Music);

        _controller = new MainMenuController(
            menuModel,
            menuView,
            services.SceneLoader,
            services.GameStart,
            gameSceneName);
    }

    private void OnDestroy()
    {
        _controller?.Dispose();
        _musicSettingsController?.Dispose();
    }
}