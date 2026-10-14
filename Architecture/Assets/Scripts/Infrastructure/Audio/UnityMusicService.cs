using System;
using UnityEngine;

public sealed class UnityMusicService : IMusicService
{
    private readonly AudioSource _source;

    public float Volume => _source.volume;

    public UnityMusicService(
        AudioSource source,
        AudioClip musicClip)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (musicClip == null)
            throw new ArgumentNullException(nameof(musicClip));

        _source = source;

        _source.clip = musicClip;
        _source.playOnAwake = false;
        _source.loop = true;
        _source.spatialBlend = 0f;
    }

    public void Play()
    {
        if (!_source.isPlaying)
            _source.Play();
    }

    public void SetVolume(float volume)
    {
        _source.volume = Mathf.Clamp01(volume);
    }
}