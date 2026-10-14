using System;

public sealed class MusicSettingsModel
{
    public event Action Changed;

    public float Volume { get; private set; }

    public MusicSettingsModel(float volume)
    {
        SetVolume(volume);
    }

    public void SetVolume(float volume)
    {
        float clamped = Math.Max(0f, Math.Min(1f, volume));

        if (Volume == clamped)
            return;

        Volume = clamped;
        Changed?.Invoke();
    }
}