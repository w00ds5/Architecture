using UnityEngine;

public sealed class GameVictoryController
{
    private readonly EnemyDefeatTracker _enemies;
    private readonly CharacterHealth _playerHealth;
    private readonly GameOverView _view;
    private readonly PauseMenuModel _pauseModel;
    private readonly MonoBehaviour[] _gameplayBehaviours;

    private float _remainingDelay;

    public bool HasWon { get; private set; }

    public GameVictoryController(
        EnemyDefeatTracker enemies,
        CharacterHealth playerHealth,
        GameOverView view,
        PauseMenuModel pauseModel,
        MonoBehaviour[] gameplayBehaviours,
        float delay)
    {
        _enemies = enemies;
        _playerHealth = playerHealth;
        _view = view;
        _pauseModel = pauseModel;
        _gameplayBehaviours = gameplayBehaviours;
        _remainingDelay = Mathf.Max(0f, delay);
    }

    public void Tick(float deltaTime)
    {
        if (HasWon
            || _playerHealth.IsDead
            || _pauseModel.IsOpen
            || deltaTime <= 0f
            || !_enemies.AllDefeated)
        {
            return;
        }

        _remainingDelay -= deltaTime;

        if (_remainingDelay <= 0f)
            CompleteVictory();
    }

    private void CompleteVictory()
    {
        _pauseModel.SetOpen(false);
        HasWon = true;

        foreach (MonoBehaviour behaviour in _gameplayBehaviours)
        {
            if (behaviour != null)
                behaviour.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        _view.Show("WIN");
    }
}