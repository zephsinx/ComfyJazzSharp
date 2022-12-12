using ComfyJazzSharp.Models;

namespace ComfyJazzSharp;

/// <summary>
///
/// </summary>
public class ComfyJazzSharp : IComfyJazzSharp
{
    #region Private Properties

    private readonly ComfyJazzSharpConfig _sharpConfig;
    private int _noteCount = 0;
    private long _lastNoteTime = 0;
    private int _currentScaleProgression = 0;

    #endregion

    #region Public Methods

    public ComfyJazzSharp(ComfyJazzSharpConfig sharpConfig)
    {
        this._sharpConfig = sharpConfig;
    }

    /// <inheritdoc cref="SetVolume"/>
    public void SetVolume(double volume)
    {
        this._sharpConfig.Volume = volume;
    }

    /// <inheritdoc cref="Mute"/>
    public void Mute()
    {
        this._sharpConfig.Volume = 0;
    }

    /// <inheritdoc cref="Unmute"/>
    public void Unmute(double volume = 1.0)
    {
        this._sharpConfig.Volume = volume;
    }

    /// <inheritdoc cref="IsMuted"/>
    public bool IsMuted()
    {
        return this._sharpConfig.Volume <= 0;
    }

    /// <inheritdoc cref="Start"/>
    public async Task<bool> Start()
    {
        DateTimeOffset startTime = DateTimeOffset.Now;
        
        // Start background loop
        // todo: Remove await when ready
        await PlayBackgroundSound($"{this._sharpConfig.SoundFolderPath}/{this._sharpConfig.BackgroundLoopPath}");

        return true;
    }

    /// <inheritdoc cref="PlayNoteProgression"/>
    public void PlayNoteProgression()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="PlayNote"/>
    public void PlayNote()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private Methods

    private Task PlayBackgroundSound(string path)
    {
        throw new NotImplementedException();
    }

    private void GetNextNote()
    {
        // TODO: Determine why '> 900'.
        if (DateTimeOffset.Now.Ticks - this._lastNoteTime > 900 || this._noteCount > Constants.MaxNotesPerPattern)
        {
            ChangePattern();
            this._noteCount = 0;
        }

        ScaleProgression scaleProgression = Constants.ScaleProgressions[this._currentScaleProgression];
        Scale scale = scaleProgression.Scale;
    }

    private void ChangePattern()
    {
        throw new NotImplementedException();
    }

    #endregion
}
