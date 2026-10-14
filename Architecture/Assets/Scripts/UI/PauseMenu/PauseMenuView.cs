using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class PauseMenuView : MonoBehaviour, IPauseMenuView
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private TMP_Text statusText;

    public event Action ResumeRequested;
    public event Action MainMenuRequested;
    public event Action SaveRequested;
    public event Action LoadRequested;

    private PauseMenuModel _model;

    public void Initialize(PauseMenuModel model)
    {
        if (_model != null)
            return;

        if (panel == null
            || resumeButton == null
            || mainMenuButton == null
            || saveButton == null
            || loadButton == null
            || statusText == null)
        {
            throw new InvalidOperationException(
                "Назначь панель, четыре кнопки и текст результата "
                + "в PauseMenuView.");
        }

        _model = model
            ?? throw new ArgumentNullException(nameof(model));

        _model.Changed += Refresh;

        resumeButton.onClick.AddListener(RequestResume);
        mainMenuButton.onClick.AddListener(RequestMainMenu);
        saveButton.onClick.AddListener(RequestSave);
        loadButton.onClick.AddListener(RequestLoad);

        Refresh();
    }

    private void OnDestroy()
    {
        if (_model == null)
            return;

        _model.Changed -= Refresh;

        resumeButton.onClick.RemoveListener(RequestResume);
        mainMenuButton.onClick.RemoveListener(RequestMainMenu);
        saveButton.onClick.RemoveListener(RequestSave);
        loadButton.onClick.RemoveListener(RequestLoad);
    }

    private void Refresh()
    {
        panel.SetActive(_model.IsOpen);
        statusText.text = _model.StatusMessage;
    }

    private void RequestResume()
    {
        ResumeRequested?.Invoke();
    }

    private void RequestMainMenu()
    {
        MainMenuRequested?.Invoke();
    }

    private void RequestSave()
    {
        SaveRequested?.Invoke();
    }

    private void RequestLoad()
    {
        LoadRequested?.Invoke();
    }
}