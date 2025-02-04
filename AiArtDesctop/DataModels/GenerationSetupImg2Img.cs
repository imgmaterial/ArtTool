using System.Text.Json.Serialization;

namespace AiArtDesctop.DataModels;

public class GenerationSetupImg2Img
{
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    [JsonPropertyName("seed")]
    public int Seed { get; set; }
    [JsonPropertyName("sampling_steps")]
    public int SamplingSteps { get; set; }
    [JsonPropertyName("hex_string")]
    public string HexString { get; set; }

    public GenerationSetupImg2Img(string prompt, int seed, int samplingSteps, string hexString)
    {
        this.Prompt = prompt;
        this.Seed = seed;
        this.SamplingSteps = samplingSteps;
        this.HexString = hexString;
    }
}