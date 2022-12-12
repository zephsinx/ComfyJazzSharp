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
    private long _autoNoteDelay;
    private double _autoNotesChance;
    private double _volume;

    public string SoundFolderPath
    {
        get => string.IsNullOrWhiteSpace(this._soundFolderPath) ? Constants.SoundFolderPath : this._soundFolderPath;
        set
        {
            ValidateString(nameof(this.SoundFolderPath), value);
            this._soundFolderPath = value.Trim('/', '\\', ' ');
        }
    }

    public string Instrument
    {
        get => string.IsNullOrWhiteSpace(this._instrument) ? Constants.Instrument : this._instrument;
        set
        {
            ValidateString(nameof(this.Instrument), value);
            this._instrument = value.Trim();
        }
    }

    public bool PlayBackgroundLoop { get; set; }

    public string BackgroundLoopPath
    {
        get => string.IsNullOrWhiteSpace(this._backgroundLoopPath) ? Constants.BackgroundLoopPath : this._backgroundLoopPath;
        set
        {
            ValidateString(nameof(this.BackgroundLoopPath), value);
            this._backgroundLoopPath = value.Trim('/', '\\', ' ');
        }
    }

    // TODO: Could it be calculated instead? Would we want to?
    public double BackgroundLoopDuration
    {
        get => this._backgroundLoopDuration;
        set
        {
            ValidateNumber(nameof(this.BackgroundLoopDuration), value);
            this._backgroundLoopDuration = value;
        }
    }

    public long AutoNoteDelay
    {
        get => this._autoNoteDelay;
        set
        {
            ValidateNumber(nameof(this.AutoNoteDelay), value);
            this._autoNoteDelay = value;
        }
    }

    public double AutoNotesChance
    {
        get => this._autoNotesChance;
        set
        {
            ValidatePercentage(nameof(this.AutoNotesChance), value);
            this._autoNotesChance = value;
        }
    }

    public bool PlayAutoNotes { get; set; }

    public double Volume
    {
        get => this._volume;
        set
        {
            ValidatePercentage(nameof(this.Volume), value);
            this._volume = value;
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
        long? autoNoteDelay = null,
        double? autoNotesChance = null,
        bool? playAutoNotes = null,
        int? volume = null)
    {
        ValidateInputs(soundFolderPath, instrument, backgroundLoopPath, backgroundLoopDuration, autoNoteDelay, autoNotesChance, volume);

        this._soundFolderPath = string.IsNullOrWhiteSpace(soundFolderPath) ? Constants.SoundFolderPath : soundFolderPath;
        this._instrument = string.IsNullOrWhiteSpace(instrument) ? Constants.Instrument : instrument;
        this.PlayBackgroundLoop = playBackgroundLoop ?? Constants.PlayBackgroundLoop;
        this._backgroundLoopPath = string.IsNullOrWhiteSpace(backgroundLoopPath) ? Constants.BackgroundLoopPath : backgroundLoopPath;
        this._backgroundLoopDuration = backgroundLoopDuration ?? Constants.BackgroundLoopDuration;
        this._autoNoteDelay = autoNoteDelay ?? Constants.AutoNoteDelay;
        this._autoNotesChance = autoNotesChance ?? Constants.AutoNotesChance;
        this.PlayAutoNotes = playAutoNotes ?? Constants.PlayAutoNotes;
        this._volume = volume ?? Constants.Volume;
    }

    private void ValidateInputs(string? soundFolderPath, string? instrument, string? backgroundLoopPath, double? backgroundLoopDuration, long? autoNoteDelay, double? autoNotesChance, int? volume)
    {
        if (soundFolderPath != null)
            ValidateString(nameof(this.SoundFolderPath), soundFolderPath);
        if (instrument != null)
            ValidateString(nameof(this.Instrument), instrument);
        if (backgroundLoopPath != null)
            ValidateString(nameof(this.BackgroundLoopPath), backgroundLoopPath);
        if (backgroundLoopDuration != null)
            ValidateNumber(nameof(this.BackgroundLoopDuration), backgroundLoopDuration.Value);
        if (autoNoteDelay != null)
            ValidateNumber(nameof(this.AutoNoteDelay), autoNoteDelay.Value);
        if (autoNotesChance != null)
            ValidatePercentage(nameof(this.AutoNotesChance), autoNotesChance.Value);
        if (volume != null)
            ValidatePercentage(nameof(this.Volume), volume.Value);
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