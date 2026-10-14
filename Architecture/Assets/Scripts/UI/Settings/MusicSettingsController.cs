using System;

public sealed class MusicSettingsController : IDisposable
{
    private readonly MusicSettingsModel _model;
    private readonly IMusicSettingsView _view;
    private readonly IMusicService _music;

    public MusicSettingsController(
        MusicSettingsModel model,
        IMusicSettingsView view,
        IMusicService music)
    {
        _model = model;
        _view = view;
        _music = music;

        _view.VolumeRequested += ChangeVolume;
        _model.Changed += ApplyVolume;

        ApplyVolume();
    }

    public void Dispose()
    {
        _view.VolumeRequested -= ChangeVolume;
        _model.Changed -= ApplyVolume;
    }

    private void ChangeVolume(float volume)
    {
        _model.SetVolume(volume);
    }

    private void ApplyVolume()
    {
        _music.SetVolume(_model.Volume);
    }
}