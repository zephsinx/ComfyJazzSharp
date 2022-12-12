namespace ComfyJazzSharp;

public interface IComfyJazzSharp
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="volume"></param>
    void SetVolume(double volume);

    /// <summary>
    ///
    /// </summary>
    void Mute();

    /// <summary>
    ///
    /// </summary>
    /// <param name="volume"></param>
    void Unmute(double volume = 1.0);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    bool IsMuted();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<bool> Start();

    /// <summary>
    ///
    /// </summary>
    void PlayNoteProgression();

    /// <summary>
    ///
    /// </summary>
    void PlayNote();
}