public interface IGameplayPauseService
{
    bool CanPause { get; }

    void SetPaused(bool paused);
}