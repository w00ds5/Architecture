using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuView : MonoBehaviour, IMainMenuView
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backButton;

    public event Action PlayRequested;
    public event Action SettingsRequested;
    public event Action BackRequested;

    private MainMenuModel _model;

    public void Initialize(MainMenuModel model)
    {
        if (_model != null)
            return;

        if (mainPanel == null || settingsPanel == null
            || playButton == null || settingsButton == null
            || backButton == null)
        {
            throw new InvalidOperationException(
                "Назначь панели и кнопки в MainMenuView.");
        }

        _model = model
            ?? throw new ArgumentNullException(nameof(model));

        _model.Changed += Refresh;

        playButton.onClick.AddListener(RequestPlay);
        settingsButton.onClick.AddListener(RequestSettings);
        backButton.onClick.AddListener(RequestBack);

        Refresh();
    }

    private void OnDestroy()
    {
        if (_model == null)
            return;

        _model.Changed -= Refresh;

        playButton.onClick.RemoveListener(RequestPlay);
        settingsButton.onClick.RemoveListener(RequestSettings);
        backButton.onClick.RemoveListener(RequestBack);
    }

    private void Refresh()
    {
        mainPanel.SetActive(!_model.SettingsOpen);
        settingsPanel.SetActive(_model.SettingsOpen);
    }

    private void RequestPlay()
    {
        PlayRequested?.Invoke();
    }

    private void RequestSettings()
    {
        SettingsRequested?.Invoke();
    }

    private void RequestBack()
    {
        BackRequested?.Invoke();
    }
}