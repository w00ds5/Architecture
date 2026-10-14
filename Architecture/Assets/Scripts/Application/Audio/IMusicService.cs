public interface IMusicService
{
    float Volume { get; }

    void Play();
    void SetVolume(float volume);
}