namespace ComfyJazzSharp.Test;

public class ComfyJazzTests
{
    [Fact]
    public async Task ComfyJazzRun()
    {
        var config = new ComfyJazzSharpConfig();
        var cjs = new ComfyJazzSharp(config);

        await cjs.PlaySound("soundPath", 1f, 1d);
    }
}