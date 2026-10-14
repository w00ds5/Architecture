using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class MusicSettingsView :
    MonoBehaviour,
    IMusicSettingsView
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeLabel;

    public event Action<float> VolumeRequested;

    private MusicSettingsModel _model;

    public void Initialize(MusicSettingsModel model)
    {
        if (_model != null)
            return;

        if (volumeSlider == null || volumeLabel == null)
        {
            throw new InvalidOperationException(
                "Назначь Volume Slider и Volume Label.");
        }

        _model = model
            ?? throw new ArgumentNullException(nameof(model));

        _model.Changed += Refresh;
        volumeSlider.onValueChanged.AddListener(RequestVolume);

        Refresh();
    }

    private void OnDestroy()
    {
        if (_model == null)
            return;

        _model.Changed -= Refresh;
        volumeSlider.onValueChanged.RemoveListener(RequestVolume);
    }

    private void Refresh()
    {
        volumeSlider.SetValueWithoutNotify(_model.Volume);

        int percent = Mathf.RoundToInt(_model.Volume * 100f);
        volumeLabel.text = $"Громкость музыки: {percent}%";
    }

    private void RequestVolume(float volume)
    {
        VolumeRequested?.Invoke(volume);
    }
}