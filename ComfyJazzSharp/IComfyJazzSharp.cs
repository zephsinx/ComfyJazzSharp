namespace ComfyJazzSharp;

public interface IComfyJazzSharp
{
    /// <summary>
    /// </summary>
    /// <param name="volume"></param>
    void SetVolume(double volume);

    /// <summary>
    /// </summary>
    void Mute();

    /// <summary>
    /// </summary>
    /// <param name="volume"></param>
    void Unmute(double volume = 1.0);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    bool IsMuted();

    /// <summary>
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Start(CancellationToken cancellationToken);

    /// <summary>
    /// </summary>
    /// <param name="noteCount"></param>
    void PlayNoteProgression(int noteCount = -1);

    /// <summary>
    /// </summary>
    /// <param name="soundUrl"></param>
    /// <param name="volume"></param>
    /// <param name="soundPlaybackRate"></param>
    /// <returns></returns>
    Task PlaySound(string soundUrl, double volume, double soundPlaybackRate);
}