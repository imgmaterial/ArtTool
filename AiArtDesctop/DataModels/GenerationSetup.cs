namespace AiArtDesctop.DataModels;
using System.Text.Json;
using System.Text.Json.Serialization;
/// <summary>
/// Class for holding information for next image. Should be JSON serialized to be sent to the backend.
/// </summary>
public class GenerationSetup
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
    [JsonPropertyName("sampling_steps")]
    public int SamplingSteps { get; set; }
    [JsonPropertyName("model_path")]
    public string ModelPath { get; set; }
    public GenerationSetup(string prompt, int seed,string modelPath, int samplingSteps)
    {
        this.Prompt = prompt;
        this.Seed = seed;
        this.SamplingSteps = samplingSteps;
        this.ModelPath = modelPath;
    }
}