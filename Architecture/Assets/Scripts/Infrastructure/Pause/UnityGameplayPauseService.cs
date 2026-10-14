using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnityGameplayPauseService :
    IGameplayPauseService
{
    private readonly MonoBehaviour[] _gameplayBehaviours;
    private readonly Func<bool> _canPause;

    private readonly List<MonoBehaviour> _temporarilyDisabled =
        new List<MonoBehaviour>();

    private bool _isPaused;
    private float _previousTimeScale;

    public bool CanPause => _canPause();

    public UnityGameplayPauseService(
        MonoBehaviour[] gameplayBehaviours,
        Func<bool> canPause)
    {
        _gameplayBehaviours = gameplayBehaviours
            ?? throw new ArgumentNullException(
                nameof(gameplayBehaviours));

        _canPause = canPause
            ?? throw new ArgumentNullException(nameof(canPause));
    }

    public void SetPaused(bool paused)
    {
        if (_isPaused == paused)
            return;

        if (paused && !CanPause)
            return;

        _isPaused = paused;

        if (paused)
            Pause();
        else
            Resume();
    }

    private void Pause()
    {
        _previousTimeScale = Time.timeScale;
        _temporarilyDisabled.Clear();

        foreach (MonoBehaviour behaviour in _gameplayBehaviours)
        {
            if (behaviour == null || !behaviour.enabled)
                continue;

            _temporarilyDisabled.Add(behaviour);
            behaviour.enabled = false;
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Resume()
    {
        Time.timeScale = _previousTimeScale;

        bool canResumeGameplay = CanPause;

        if (canResumeGameplay)
        {
            foreach (MonoBehaviour behaviour in _temporarilyDisabled)
            {
                if (behaviour != null)
                    behaviour.enabled = true;
            }
        }

        _temporarilyDisabled.Clear();

        Cursor.lockState = canResumeGameplay
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !canResumeGameplay;
    }
}