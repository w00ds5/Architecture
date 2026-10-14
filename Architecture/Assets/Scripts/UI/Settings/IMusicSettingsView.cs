using System;

public interface IMusicSettingsView
{
    event Action<float> VolumeRequested;
}