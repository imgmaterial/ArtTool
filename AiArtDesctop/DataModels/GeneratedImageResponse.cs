namespace AiArtDesctop.DataModels;
using System.Text.Json;
using System.Text.Json.Serialization;
public class GeneratedImageResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("format")]
    public string Format { get; set; }
    [JsonPropertyName("image_bytes")]
    public string ImageBytes { get; set; } // Hex-encoded string
}