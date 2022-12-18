namespace ComfyJazzSharp;

/// <summary>
///
/// </summary>
public sealed class ComfyJazzSharpConfig
{
    private string _soundFolderPath;
    private string _instrument;
    private string _backgroundLoopPath;
    private double _backgroundLoopDuration;
    private int _autoNoteDelay;
    private double _autoNotesChance;
    private double _volume;

    public string SoundFolderPath
    {
        get => string.IsNullOrWhiteSpace(_soundFolderPath) ? Constants.SoundFolderPath : _soundFolderPath;
        set
        {
            ValidateString(nameof(SoundFolderPath), value);
            _soundFolderPath = value.Trim('/', '\\', ' ');
        }
    }

    public string Instrument
    {
        get => string.IsNullOrWhiteSpace(_instrument) ? Constants.Instrument : _instrument;
        set
        {
            ValidateString(nameof(Instrument), value);
            _instrument = value.Trim();
        }
    }

    public bool PlayBackgroundLoop { get; set; }

    public string BackgroundLoopPath
    {
        get => string.IsNullOrWhiteSpace(_backgroundLoopPath) ? Constants.BackgroundLoopPath : _backgroundLoopPath;
        set
        {
            ValidateString(nameof(BackgroundLoopPath), value);
            _backgroundLoopPath = value.Trim('/', '\\', ' ');
        }
    }

    // todo: Could it be calculated instead? Would we want to?
    public double BackgroundLoopDuration
    {
        get => _backgroundLoopDuration;
        set
        {
            ValidateNumber(nameof(BackgroundLoopDuration), value);
            _backgroundLoopDuration = value;
        }
    }

    public int AutoNoteDelay
    {
        get => _autoNoteDelay;
        set
        {
            ValidateNumber(nameof(AutoNoteDelay), value);
            _autoNoteDelay = value;
        }
    }

    public double AutoNotesChance
    {
        get => _autoNotesChance;
        set
        {
            ValidatePercentage(nameof(AutoNotesChance), value);
            _autoNotesChance = value;
        }
    }

    public bool PlayAutoNotes { get; set; }

    public double Volume
    {
        get => _volume;
        set
        {
            ValidatePercentage(nameof(Volume), value);
            _volume = value;
        }
    }

    /// <summary>
    /// Initialize ComfyJazzSharpConfig.
    /// </summary>
    /// <param name="soundFolderPath">Root path of folder where sound files are stored.</param>
    /// <param name="instrument">Comma separated list of instruments to play.</param>
    /// <param name="playBackgroundLoop">Whether a background loop should be played.</param>
    /// <param name="backgroundLoopPath">Path to background loop track relative to the root folder.</param>
    /// <param name="backgroundLoopDuration">Duration of background loop track in seconds.</param>
    /// <param name="autoNoteDelay">Delay in milliseconds between auto-notes.</param>
    /// <param name="autoNotesChance">% chance to play auto-notes.</param>
    /// <param name="playAutoNotes">Whether auto-notes should be played.</param>
    /// <param name="volume">Volume at which to play notes.</param>
    public ComfyJazzSharpConfig(
        string? soundFolderPath = null,
        string? instrument = null,
        bool? playBackgroundLoop = null,
        string? backgroundLoopPath = null,
        double? backgroundLoopDuration = null,
        int? autoNoteDelay = null,
        double? autoNotesChance = null,
        bool? playAutoNotes = null,
        int? volume = null)
    {
        ValidateInputs(soundFolderPath, instrument, backgroundLoopPath, backgroundLoopDuration, autoNoteDelay, autoNotesChance, volume);

        _soundFolderPath = string.IsNullOrWhiteSpace(soundFolderPath) ? Constants.SoundFolderPath : soundFolderPath;
        _instrument = string.IsNullOrWhiteSpace(instrument) ? Constants.Instrument : instrument;
        PlayBackgroundLoop = playBackgroundLoop ?? Constants.PlayBackgroundLoop;
        _backgroundLoopPath = string.IsNullOrWhiteSpace(backgroundLoopPath) ? Constants.BackgroundLoopPath : backgroundLoopPath;
        _backgroundLoopDuration = backgroundLoopDuration ?? Constants.BackgroundLoopDuration;
        _autoNoteDelay = autoNoteDelay ?? Constants.AutoNoteDelay;
        _autoNotesChance = autoNotesChance ?? Constants.AutoNotesChance;
        PlayAutoNotes = playAutoNotes ?? Constants.PlayAutoNotes;
        _volume = volume ?? Constants.Volume;
    }

    private void ValidateInputs(string? soundFolderPath, string? instrument, string? backgroundLoopPath, double? backgroundLoopDuration, long? autoNoteDelay, double? autoNotesChance, int? volume)
    {
        if (soundFolderPath != null)
            ValidateString(nameof(SoundFolderPath), soundFolderPath);
        if (instrument != null)
            ValidateString(nameof(Instrument), instrument);
        if (backgroundLoopPath != null)
            ValidateString(nameof(BackgroundLoopPath), backgroundLoopPath);
        if (backgroundLoopDuration != null)
            ValidateNumber(nameof(BackgroundLoopDuration), backgroundLoopDuration.Value);
        if (autoNoteDelay != null)
            ValidateNumber(nameof(AutoNoteDelay), autoNoteDelay.Value);
        if (autoNotesChance != null)
            ValidatePercentage(nameof(AutoNotesChance), autoNotesChance.Value);
        if (volume != null)
            ValidatePercentage(nameof(Volume), volume.Value);
    }

    private static void ValidateString(string paramName, string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(paramName, $"{paramName} must not be null or empty");
    }

    private static void ValidateNumber(string paramName, double value)
    {
        if (value < 0.0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be a positive number");
    }

    private static void ValidatePercentage(string paramName, double value)
    {
        if (value is < 0.0 or > 1.0)
            throw new ArgumentOutOfRangeException(paramName, $"{paramName} must be a positive number between 0.0 and 1.0");
    }
}