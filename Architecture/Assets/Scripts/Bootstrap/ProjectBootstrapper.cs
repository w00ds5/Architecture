using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public sealed class ProjectBootstrapper : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip musicClip;

    private static ProjectBootstrapper _instance;

    private ProjectServices _services;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        ISceneLoader sceneLoader = new UnitySceneLoader();

        IMusicService music = new UnityMusicService(
            musicSource,
            musicClip);

        string savePath = Path.Combine(
            Application.persistentDataPath,
            "save.json");

        IGameSaveRepository repository =
            new JsonFileGameSaveRepository(savePath);

        GameSaveDataValidator validator =
            new GameSaveDataValidator();

        IGameSaveInteractor saves =
            new GameSaveInteractor(repository, validator);

        IGameStartState gameStart = new GameStartState();
        IGameMode gameMode = new GameMode();

        _services = new ProjectServices(
            sceneLoader,
            music,
            saves,
            gameStart,
            gameMode);

        SceneManager.sceneLoaded += InitializeScene;
    }

    private void Start()
    {
        if (_instance != this)
            return;

        _services.Music.Play();
        _services.SceneLoader.Load(mainMenuSceneName);
    }

    private void OnDestroy()
    {
        if (_instance != this)
            return;

        SceneManager.sceneLoaded -= InitializeScene;
        _instance = null;
    }

    private void InitializeScene(
        Scene scene,
        LoadSceneMode loadMode)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            MonoBehaviour[] components =
                root.GetComponentsInChildren<MonoBehaviour>(true);

            foreach (MonoBehaviour component in components)
            {
                if (component is ISceneBootstrapper bootstrapper
                    && component.isActiveAndEnabled)
                {
                    bootstrapper.Initialize(_services);
                }
            }
        }
    }
}