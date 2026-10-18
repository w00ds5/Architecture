using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameModeSettingsView :
    MonoBehaviour,
    IGameModeSettingsView
{
    [SerializeField] private Toggle peacefulToggle;

    public event Action<bool> PeacefulRequested;

    private IGameMode _mode;

    public void Initialize(IGameMode mode)
    {
        if (_mode != null)
            return;

        if (peacefulToggle == null)
        {
            throw new InvalidOperationException(
                "Назначь Peaceful Toggle.");
        }

        _mode = mode
            ?? throw new ArgumentNullException(nameof(mode));

        _mode.Changed += Refresh;

        peacefulToggle.onValueChanged.AddListener(
            RequestPeaceful);

        Refresh();
    }

    private void OnDestroy()
    {
        if (_mode == null)
            return;

        _mode.Changed -= Refresh;

        peacefulToggle.onValueChanged.RemoveListener(
            RequestPeaceful);
    }

    private void Refresh()
    {
        peacefulToggle.SetIsOnWithoutNotify(
            _mode.IsPeaceful);
    }

    private void RequestPeaceful(bool peaceful)
    {
        PeacefulRequested?.Invoke(peaceful);
    }
}