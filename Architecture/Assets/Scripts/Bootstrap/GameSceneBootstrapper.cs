using UnityEngine;

[DefaultExecutionOrder(-200)]
[RequireComponent(typeof(PauseMenuInput))]
public sealed class GameSceneBootstrapper :
    MonoBehaviour,
    ISceneBootstrapper
{
    [SerializeField] private CharacterHealth playerHealth;
    [SerializeField] private PauseMenuView pauseView;
    [SerializeField] private MonoBehaviour[] gameplayBehaviours;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemySaveIdentity meleePrefab;
    [SerializeField] private EnemySaveIdentity rangedPrefab;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private EnemySaveIdentity bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private GameOverView gameEndView;

    [SerializeField, Min(0f)]
    private float victoryDelay = 4.7f;


    private EnemyDefeatTracker _enemyDefeatTracker;
    private GameVictoryController _victoryController;
    private PauseMenuController _pauseController;
    private IEnemyFactory _enemyFactory;
    private IGameStateSource _gameStateSource;
    private PauseSaveLoadController _saveLoadController;

    public void Initialize(ProjectServices services)
    {
        if (_pauseController != null)
            return;

        if (playerHealth == null
            || pauseView == null
            || gameplayBehaviours == null
            || gameplayBehaviours.Length == 0
            || enemySpawner == null
            || meleePrefab == null
            || rangedPrefab == null
            || enemyParent == null
            || bossPrefab == null
            || gameEndView == null
            || bossSpawnPoint == null)
        {
            Debug.LogError(
                "Назначь игрока, меню паузы, управление, "
                + "спавнер, префабы и родителя противников.",
                this);

            return;
        }

        UnityCharacterSaveState playerState =
            playerHealth.GetComponent<UnityCharacterSaveState>();

        if (playerState == null)
        {
            Debug.LogError(
                "Добавь UnityCharacterSaveState на корень игрока.",
                playerHealth);

            return;
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _enemyFactory = new UnityEnemyFactory(
            meleePrefab,
            rangedPrefab,
            bossPrefab,
            playerHealth,
            enemyParent,
            services.GameMode);

        _gameStateSource = new UnityGameStateSource(
            playerState,
            enemyParent);

        if (services.GameStart.TryTakeLoad(out GameSaveData savedGame))
        {
            UnityGameStateRestorer restorer = new(
                playerState,
                _enemyFactory);

            restorer.Restore(savedGame);
        }
        else
        {
            _enemyFactory.Create(
                EnemyKind.Boss,
                bossSpawnPoint.position,
                bossSpawnPoint.eulerAngles.y);

            enemySpawner.SpawnRandom(_enemyFactory);
        }

        _enemyDefeatTracker = new EnemyDefeatTracker(
        enemyParent.GetComponentsInChildren<CharacterHealth>());

        PauseMenuInput input = GetComponent<PauseMenuInput>();

        IGameplayPauseService pauseService =
            new UnityGameplayPauseService(
                gameplayBehaviours,
                () => playerHealth != null
                    && !playerHealth.IsDead
                    && (_victoryController == null
                        || !_victoryController.HasWon));

        PauseMenuModel model = new PauseMenuModel();
        pauseView.Initialize(model);

        _pauseController = new PauseMenuController(
            model,
            pauseView,
            input,
            pauseService,
            services.SceneLoader,
            mainMenuSceneName);

        _saveLoadController = new PauseSaveLoadController(
        model,
        pauseView,
        services.Saves,
        _gameStateSource,
        services.GameStart,
        services.SceneLoader,
        gameObject.scene.name,
        error => Debug.LogException(error, this));

        _victoryController = new GameVictoryController(
        _enemyDefeatTracker,
        playerHealth,
        gameEndView,
        model,
        gameplayBehaviours,
        victoryDelay);
    }

    private void Update()
    {
        _pauseController?.Tick();
        _victoryController?.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        _saveLoadController?.Dispose();
        _pauseController?.Dispose();
        _enemyDefeatTracker?.Dispose();
    }
}