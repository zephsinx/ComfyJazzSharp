using ComfyJazzSharp.Models;

namespace ComfyJazzSharp;

public static class Constants
{
    // Default values
    public const string SoundFolderPath = "web/sounds";
    public const string Instrument = "piano";
    public const bool PlayBackgroundLoop = true;
    public const string BackgroundLoopPath = "jazz_loop.ogg";
    public const double BackgroundLoopDuration = 27.428;
    public const long AutoNoteDelay = 300;
    public const double AutoNotesChance = 0.2;
    public const bool PlayAutoNotes = true;
    public const double Volume = 1.0;

    // Properties
    public const int MaxNotesPerPattern = 30;

    public static ScaleProgression[] ScaleProgressions =
    {
        new()
        {
            Start = 0,
            End = 3.428,
            Scale = Scale.Custom,
            TargetNotes = new[] { 2, 4, 7 },
            Root = 7,
        },
        new()
        {
            Start = 3.428,
            End = 6.857,
            Scale = Scale.Diatonic,
            TargetNotes = new[] { 2, 4, 7 },
            Root = 2,
        },
        new()
        {
            Start = 6.857,
            End = 10.285,
            Scale = Scale.Custom,
            TargetNotes = new[] { 2, 4, 7 },
            Root = 7,
        },
        new()
        {
            Start = 10.285,
            End = 12,
            Scale = Scale.Diatonic,
            TargetNotes = new[] { 4, 5, 9 },
            Root = 9,
        },
        new()
        {
            Start = 12,
            End = 13.714,
            Scale = Scale.Custom2,
            TargetNotes = new[] { 2, 4, 11 },
            Root = 2,
        },
        new()
        {
            Start = 13.714,
            End = 17.142,
            Scale = Scale.Custom,
            TargetNotes = new[] { 4, 7, 11 },
            Root = 11,
        },
        new()
        {
            Start = 17.142,
            End = 20.571,
            Scale = Scale.Custom,
            TargetNotes = new[] { 0, 2, 4 },
            Root = 4,
        },
        new()
        {
            Start = 20.571,
            End = 24,
            Scale = Scale.Diatonic,
            TargetNotes = new[] { 4, 5, 9 },
            Root = 9,
        },
        new()
        {
            Start = 24,
            End = 27.428,
            Scale = Scale.Custom2,
            TargetNotes = new[] { 2, 4, 11 },
            Root = 2,
        },
    };

    public static Dictionary<Scale, int[]> Scales = new()
    {
        { Scale.Diatonic, new[] { 0, -1, 0, -1, 0, 0, -1, 0, -1, 0, -1, 0 } },
        { Scale.Dorian, new[] { 0, 1, 0, 0, -1, 0, 1, 0, 1, 0, 0, -1 } },
        { Scale.Phrygian, new[] { 0, 0, -1, 0, -1, 0, 1, 0, 0, -1, 0, -1 } },
        { Scale.Lydian, new[] { 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0 } },
        { Scale.Mixolydian, new[] { 0, 1, 0, 1, 0, 0, -1, 0, -1, 0, 0, -1 } },
        { Scale.Aeolian, new[] { 0, -1, 0, 0, -1, 0, -1, 0, 0, -1, 0, -1 } },
        { Scale.Locrian, new[] { 0, 0, -1, 0, -1, 0, 0, -1, 0, -1, 0, -1 } },
        { Scale.HarmonicMinor, new[] { 0, 1, 0, 0, -1, 0, 1, 0, 0, -1, 1, 0 } },
        { Scale.MelodicMinor, new[] { 0, 1, 0, 0, -1, 0, 1, 0, -1, 0, 1, 0 } },
        { Scale.MajorPentatonic, new[] { 0, 1, 0, 1, 0, -1, 1, 0, 1, 0, -1, 1 } },
        { Scale.MinorPentatonic, new[] { 0, -1, 1, 0, -1, 0, 1, 0, -1, 1, 0, -1 } },
        { Scale.DoubleHarmonic, new[] { 0, 0, -1, 1, 0, 0, 1, 0, 0, -1, 1, 0 } },
        { Scale.HalfDim, new[] { 0, 1, 0, 0, -1, 0, 0, -1, 0, -1, 0, -1 } },
        { Scale.Chromatic, new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { Scale.Custom, new[] { 0, -1, 0, -1, 0, -1, -1, 0, -1, 0, -1, 0 } },
        { Scale.Custom2, new[] { -1, 0, 0, -1, 0, 0, 1, 0, 0, -1, 0, 0 } },
    };

    public static List<int[]> Patterns = new()
    {
        new[] { 71, 72, 69, 71, 67, 69, 64, 67, 62, 64, 62, 60, 59, 60, 62, 64, 65, 67, 69, 71, 67, 64, 62, 60, 59, 60, 57, 59, 55 },
        new[] { 83, 88, 86, 81, 79, 83, 81, 76, 74, 79, 76, 72, 71, 72, 69, 67 },
        new[] { 74, 72, 70, 69, 70, 67, 69, 65, 67, 62, 65, 63, 67, 70, 74, 77, 74, 77, 74, 72, 70, 69, 70, 67, 69, 65 },
        new[] { 69, 74, 72, 67, 64, 69, 67, 62, 60, 64, 62, 57, 55, 60, 57, 53, 55, 57, 60, 62, 64, 65, 67, 62, 65, 64, 62, 64, 62, 60, 59 },
        new[] { 59, 60, 64, 67, 71, 72, 76, 79, 83, 84, 88, 91, 95, 98, 95, 98, 95, 91, 88, 91, 88, 84, 83, 86, 83, 79, 76, 79, 76, 72, 71, 74, 71, 67, 64, 67, 64, 60, 59, 55 },
        new[] { 91, 86, 88, 84, 83, 86, 83, 79, 76, 79, 76, 72, 71, 74, 71, 67, 64, 67, 64, 60, 59, 60, 64, 67, 71, 72, 74, 76, 79, 74, 76, 71, 72, 67 },
        new[] { 67, 65, 64, 65, 69, 72, 76, 79, 77, 76, 74, 76, 72, 71, 74, 71, 72, 67, 64, 67, 62, 60 },
        new[] { 65, 67, 65, 64, 65, 67, 69, 71, 72, 74, 76, 77, 79, 81, 83, 84, 86, 88, 89, 91, 93, 91, 88, 86, 88, 86, 84, 83, 84, 79, 81, 76, 79, 74, 76, 72, 71, 71, 72, 67 },
        new[] { 55, 59, 60, 62, 67, 71, 72, 76, 79, 83, 86, 88, 93, 91, 88, 84, 81, 79, 77, 76, 74, 72, 71 },
    };

    public static Note[] Notes =
    {
        new()
        {
            Name = "note_96",
            NoteMetadata = new NoteMetadata
            {
                Root = 96,
                StartRange = 95,
                EndRange = 127,
            },
        },
        new()
        {
            Name = "note_93",
            NoteMetadata = new NoteMetadata
            {
                Root = 93,
                StartRange = 92,
                EndRange = 94,
            },
        },
        new()
        {
            Name = "note_90",
            NoteMetadata = new NoteMetadata
            {
                Root = 90,
                StartRange = 89,
                EndRange = 91,
            },
        },
        new()
        {
            Name = "note_87",
            NoteMetadata = new NoteMetadata
            {
                Root = 87,
                StartRange = 86,
                EndRange = 88,
            },
        },
        new()
        {
            Name = "note_84",
            NoteMetadata = new NoteMetadata
            {
                Root = 84,
                StartRange = 83,
                EndRange = 85,
            },
        },
        new()
        {
            Name = "note_81",
            NoteMetadata = new NoteMetadata
            {
                Root = 81,
                StartRange = 80,
                EndRange = 82,
            },
        },
        new()
        {
            Name = "note_77",
            NoteMetadata = new NoteMetadata
            {
                Root = 78,
                StartRange = 77,
                EndRange = 79,
            },
        },
        new()
        {
            Name = "note_74",
            NoteMetadata = new NoteMetadata
            {
                Root = 75,
                StartRange = 74,
                EndRange = 76,
            },
        },
        new()
        {
            Name = "note_71",
            NoteMetadata = new NoteMetadata
            {
                Root = 72,
                StartRange = 71,
                EndRange = 73,
            },
        },
        new()
        {
            Name = "note_69",
            NoteMetadata = new NoteMetadata
            {
                Root = 69,
                StartRange = 68,
                EndRange = 70,
            },
        },
        new()
        {
            Name = "note_66",
            NoteMetadata = new NoteMetadata
            {
                Root = 66,
                StartRange = 65,
                EndRange = 67,
            },
        },
        new()
        {
            Name = "note_63",
            NoteMetadata = new NoteMetadata
            {
                Root = 63,
                StartRange = 62,
                EndRange = 64,
            },
        },
        new()
        {
            Name = "note_60",
            NoteMetadata = new NoteMetadata
            {
                Root = 60,
                StartRange = 59,
                EndRange = 61,
            },
        },
        new()
        {
            Name = "note_57",
            NoteMetadata = new NoteMetadata
            {
                Root = 57,
                StartRange = 56,
                EndRange = 58,
            },
        },
        new()
        {
            Name = "note_54",
            NoteMetadata = new NoteMetadata
            {
                Root = 54,
                StartRange = 53,
                EndRange = 55,
            },
        },
        new()
        {
            Name = "note_51",
            NoteMetadata = new NoteMetadata
            {
                Root = 51,
                StartRange = 50,
                EndRange = 52,
            },
        },
        new()
        {
            Name = "note_48",
            NoteMetadata = new NoteMetadata
            {
                Root = 48,
                StartRange = 0,
                EndRange = 49,
            },
        },
    };
}