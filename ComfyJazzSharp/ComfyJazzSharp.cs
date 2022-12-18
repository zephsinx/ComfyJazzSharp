using ComfyJazzSharp.Models;
using NAudio.Wave;

// Disable "call not awaited" warning, as application relies on them.
#pragma warning disable CS4014

namespace ComfyJazzSharp;

/// <summary>
///
/// </summary>
public class ComfyJazzSharp : IComfyJazzSharp
{
    #region Private Properties

    private readonly ComfyJazzSharpConfig _sharpConfig;

    private readonly Random _random;

    /// <summary>
    /// Dictionary to store the cached length of notes.
    /// </summary>
    /// <remarks>Key is the instrument name, and value is a second dictionary of note names to note duration.</remarks>
    private Dictionary<string, Dictionary<string, double>> _noteDuration;

    private const int Transpose = -5;

    private int _noteCount;
    private long _lastNoteTime;
    private int _lastNoteIndex;
    private int _currentScaleProgression;
    private int _currentStep;
    private int _lastRoot = -1;
    private int _pattern = -1;

    private Scale _scale = Scale.Custom;

    #endregion

    #region Public Methods

    public ComfyJazzSharp(ComfyJazzSharpConfig sharpConfig)
    {
        _sharpConfig = sharpConfig;
        _random = new Random(GetSeedFromTime());

        _noteDuration = new();
        PopulateNoteDurationCache();
    }

    /// <inheritdoc cref="SetVolume"/>
    public void SetVolume(double volume)
    {
        _sharpConfig.Volume = volume;
    }

    /// <inheritdoc cref="Mute"/>
    public void Mute()
    {
        _sharpConfig.Volume = 0;
    }

    /// <inheritdoc cref="Unmute"/>
    public void Unmute(double volume = 1.0)
    {
        _sharpConfig.Volume = volume;
    }

    /// <inheritdoc cref="IsMuted"/>
    public bool IsMuted()
    {
        return _sharpConfig.Volume <= 0;
    }

    /// <inheritdoc cref="Start"/>
    public async Task Start(CancellationToken cancellationToken)
    {
        long startTime = GetCurrentMilliseconds();
        
        // Start background loop
        PlaySound($"{_sharpConfig.SoundFolderPath}/{_sharpConfig.BackgroundLoopPath}", _sharpConfig.Volume, 1.0);

        var random = new Random();

        while (true)
        {
            long currentTimeSeconds = (GetCurrentMilliseconds() - startTime) / 1000;

            if (currentTimeSeconds > _sharpConfig.BackgroundLoopDuration)
            {
                startTime = GetCurrentMilliseconds();
                currentTimeSeconds = 0;

                PlaySound($"{_sharpConfig.SoundFolderPath}/{_sharpConfig.BackgroundLoopPath}", _sharpConfig.Volume, 1.0);
            }

            for (int i = 0; i < Constants.ScaleProgressions.Length; i++)
            {
                if (!(Constants.ScaleProgressions[i].Start <= currentTimeSeconds) || !(currentTimeSeconds <= Constants.ScaleProgressions[i].End))
                    continue;

                _currentScaleProgression = i;
                break;
            }

            if (_sharpConfig.PlayAutoNotes && random.NextDouble() < _sharpConfig.AutoNotesChance)
            {
                PlayNoteRandomly();
            }

            await Task.Delay(_sharpConfig.AutoNoteDelay, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return;
        }
    }

    /// <inheritdoc cref="PlayNoteProgression"/>
    public void PlayNoteProgression(int noteCount = -1)
    {
        if (noteCount < 0)
            noteCount = (int)Math.Floor(new Random().NextDouble() * 8);

        for (int i = 0; i < noteCount; i++)
        {
            PlayNoteRandomly(100, 200 * i);
        }
    }

    /// <inheritdoc cref="PlaySound"/>
    public async Task PlaySound(string soundUrl, double volume, double soundPlaybackRate)
    {
        using var audioPlayer = new SoundProcessorViewModel();
        audioPlayer.OpenFile(soundUrl);
        audioPlayer.Volume = volume;
        audioPlayer.Rate = soundPlaybackRate;

        audioPlayer.Play.Execute(null);

        var audioFile = new AudioFileReader(soundUrl);

        // todo: Store note times for each instrument in a cache/dictionary.
        await Task.Delay(audioFile.TotalTime);
    }

    #endregion

    #region Private Methods

    private async Task PlayBackgroundSound(string soundUrl, double volume, double soundPlaybackRate)
    {

    }

    private async Task PlayNoteRandomly(int minRandom = 0, int maxRandom = 200)
    {
        await Task.Delay(minRandom + (int)(maxRandom * _random.NextDouble()));

        Note sound = GetNextNote();

        string instrument = GetRandomInstrument();

        // todo: Add sound URL
        await PlaySound(soundUrl: $"{_sharpConfig.SoundFolderPath}/{_sharpConfig.BackgroundLoopPath}", _sharpConfig.Volume, sound.PlaybackRate);
    }

    private string GetRandomInstrument()
    {
        List<string> instruments = _sharpConfig.Instrument.Split(",").Select(i => i.Trim()).ToList();
        return instruments[GetRandomInt(instruments.Count)];
    }

    private Note GetNextNote()
    {
        // todo: Determine why '> 900'.
        if (GetCurrentMilliseconds() - _lastNoteTime > 900 || _noteCount > Constants.MaxNotesPerPattern)
        {
            ChangePattern();
            _noteCount = 0;
        }

        ScaleProgression scaleProgression = Constants.ScaleProgressions[_currentScaleProgression];
        _scale = scaleProgression.Scale;
        int nextNoteIndex = GetNote(_scale);

        while (nextNoteIndex == _lastNoteIndex)
        {
            nextNoteIndex = GetNote(_scale);
        }

        if (scaleProgression.Root != _lastRoot)
            nextNoteIndex = ScaleNote(nextNoteIndex, scaleProgression.TargetNotes);

        int noteRange = nextNoteIndex == -1 ? 48 : nextNoteIndex;

        Note note = Constants.Notes.Single(n => n.NoteMetadata.StartRange <= noteRange && noteRange <= n.NoteMetadata.EndRange);

        int rootOffset = noteRange - note.NoteMetadata.Root;
        note.PlaybackRate = SemitonesToPlaybackRate(rootOffset);

        _noteCount++;
        _lastNoteTime = GetCurrentMilliseconds();
        _lastNoteIndex = nextNoteIndex;
        _lastRoot = scaleProgression.Root;

        return note;
    }

    private static double SemitonesToPlaybackRate(int rootOffset)
    {
        return Math.Pow(rootOffset, Math.Pow(2, 1.0/12.0));
    }

    private int GetRandomInt(int number)
    {
        return (int)Math.Floor(number * _random.NextDouble());
    }

    private int ScaleNote(int noteIndex, int[] targetNotes)
    {
        int n = (noteIndex % 12 + 5) % 12;
        if (targetNotes.Any(tn => tn == n))
            return noteIndex;

        int r = GetClosestTarget(targetNotes, noteIndex);
        int o = (noteIndex - (n - r));
        noteIndex = o + Constants.Scales[_scale][(o % 12 + 5) % 12];

        return noteIndex;
    }

    private static int GetClosestTarget(IEnumerable<int> targetNotes, int noteIndex)
    {
        return targetNotes.Aggregate(
            0, (total, n) =>
                total + (Math.Abs(n - noteIndex) < Math.Abs(total - noteIndex) ? n : total));
    }

    private int GetNote(Scale scale)
    {
        if (_pattern < 0)
            ChangePattern();

        int stepValue = Constants.Patterns[_pattern][_currentStep];
        int scaleShift = stepValue + Constants.Scales[scale][stepValue % 12];

        int shiftedNote = Transpose + scaleShift;
        _currentStep = (_currentStep + 1) % Constants.Patterns[_pattern].Length;

        return shiftedNote;
    }

    private void ChangePattern()
    {
        _pattern = GetRandomInt(Constants.Patterns.Count);
        _currentStep = 0;
    }

    private static long GetCurrentMilliseconds()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    private static int GetSeedFromTime()
    {
        long currentMilliseconds = GetCurrentMilliseconds();

        return currentMilliseconds < int.MaxValue
            ? Convert.ToInt32(currentMilliseconds)
            : Convert.ToInt32(GetCurrentMilliseconds() - currentMilliseconds);
    }

    private void PopulateNoteDurationCache()
    {
        // todo: Populate note duration dictionary.
        _noteDuration.Add(Guid.NewGuid().ToString(), new Dictionary<string, double>());
        _noteDuration.TryGetValue("test", out _);
    }

    #endregion
}
