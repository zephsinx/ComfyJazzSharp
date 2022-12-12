namespace ComfyJazzSharp.Models;

public class Note
{
    public string Name { get; set; } = string.Empty;

    public NoteMetadata NoteMetadata { get; set; } = new();
}