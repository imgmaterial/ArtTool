using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;


public class ApiResponse
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

public class ImageGenerationService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<byte[]> GenerateImageAsync(string prompt)
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(500);
        var url = $"http://localhost:8000/generate_image/{prompt}";

        var response = await client.PostAsync(url, null);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, options);
        Console.WriteLine(apiResponse.ImageBytes);
        if (apiResponse?.ImageBytes == null)
        {
            throw new Exception("Failed to deserialize the API response.");
        }

        return Convert.FromHexString(apiResponse.ImageBytes);
    }
}