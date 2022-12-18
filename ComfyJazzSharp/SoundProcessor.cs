// using NAudio.Vorbis;
using NAudio.Wave;
using SoundTouch.Net.NAudioSupport;

namespace ComfyJazzSharp;

public sealed class SoundProcessor : IDisposable
{
    private IWavePlayer? _waveOut;

    /// <summary>
    /// Event for "playback stopped" event. 'bool' argument is true if playback has reached end of stream.
    /// </summary>
    // ReSharper disable once EventNeverSubscribedTo.Global
    public event EventHandler<bool> PlaybackStopped = (_, __) => { };

    public SoundTouchWaveStream? ProcessorStream { get; private set; }

    /// <summary>
    /// Start or resume playback.
    /// </summary>
    /// <returns>true if successful, false if audio file not open.</returns>
    public bool Play(float volume = 1f)
    {
        if (_waveOut == null || _waveOut.PlaybackState == PlaybackState.Playing)
            return false;

        _waveOut.Volume = volume;

        _waveOut.Play();
        return true;
    }

    /// <summary>
    /// Pause playback.
    /// </summary>
    /// <returns>true if successful, false if audio not playing.</returns>
    public bool Pause()
    {
        if (_waveOut is not { PlaybackState: PlaybackState.Playing })
            return false;

        _waveOut.Stop();
        return true;
    }

    /// <summary>
    /// Stop playback.
    /// </summary>
    /// <returns>true if successful, false if audio file not open.</returns>
    public bool Stop()
    {
        if (_waveOut is null || ProcessorStream is null)
            return false;

        _waveOut.Stop();
        ProcessorStream.Position = 0;
        ProcessorStream.Flush();
        return true;
    }

    /// <summary>
    /// Fade sound.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="durationMillis"></param>
    /// <returns></returns>
    public bool Fade(float from, float to, long durationMillis)
    {
        if (_waveOut is null || ProcessorStream is null)
            return false;

        float volume = from;
        float volumeDiff = to - from;
        float steps = Math.Abs(volumeDiff / 0.01f);
        float stepLen = Math.Max(4, steps > 0 ? durationMillis / steps : durationMillis);
        long lastTick = GetCurrentMilliseconds();

        for (int i = 0; i < steps; i++)
        {
            long now = GetCurrentMilliseconds();
            long tick = (now - lastTick) / durationMillis;
            lastTick = now;

            volume += volumeDiff * tick;
            volume = (float)Math.Round(volume, 2);
            volume = volumeDiff < 0 ? Math.Max(to, volume) : Math.Min(to, volume);

            _waveOut.Volume = volume;
        }

        return true;
    }

    /// <summary>
    /// Opens the specified filename for playback.
    /// </summary>
    /// <param name="filePath">Path to file to open.</param>
    /// <returns><c>true</c> if successful; otherwise <c>false</c>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed: The WaveStreams are disposed by the SoundTouchWaveStream.")]
    public bool OpenFile(string filePath)
    {
        Close();

        try
        {
            WaveStream reader = GetFileReader(filePath);

            // don't pad, otherwise the stream never ends
            var inputStream = new WaveChannel32(reader) { PadWithZeroes = false };

            ProcessorStream = new SoundTouchWaveStream(inputStream);

            _waveOut = new WaveOutEvent() { DesiredLatency = 100 };

            _waveOut.Init(ProcessorStream);
            _waveOut.PlaybackStopped += OnPlaybackStopped;

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());

            // Error in opening file
            _waveOut = null;
            return false;
        }
    }

    public void Dispose() => Close();

    /// <summary>
    ///
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private static WaveStream GetFileReader(string filePath)
    {
        string pathExtension = Path.GetExtension(filePath);

        return pathExtension switch
        {
            ".mp3" => new Mp3FileReader(filePath),
            // ".ogg" => new VorbisWaveReader(filePath),
            _ => new WaveFileReader(filePath),
        };
    }

    /// <summary>
    /// Proxy event handler for receiving playback stopped event from WaveOut.
    /// </summary>
    private void OnPlaybackStopped(object? sender, StoppedEventArgs args)
    {
        bool reachedEnding = ProcessorStream is null || ProcessorStream.Position >= ProcessorStream.Length;
        if (reachedEnding)
            _ = Stop();

        PlaybackStopped(sender, reachedEnding);
    }

    /// <summary>
    ///
    /// </summary>
    private void Close()
    {
        ProcessorStream?.Dispose();
        ProcessorStream = null;

        _waveOut?.Dispose();
        _waveOut = null;
    }

    private static long GetCurrentMilliseconds()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
}