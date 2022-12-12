namespace ComfyJazzSharp.Models;

public class ScaleProgression
{
    public double Start { get; set; }
    public double End { get; set; }
    public Scale Scale { get; set; }
    public int[] TargetNotes { get; set; } = Array.Empty<int>();
    public int Root { get; set; }
}