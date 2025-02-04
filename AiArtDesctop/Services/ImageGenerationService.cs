using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AiArtDesctop.DataModels;


public class ImageGenerationService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<byte[]> GenerateImageAsync(GenerationSetup imageSetup)
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(500);
        var url = $"http://localhost:8000/generate_image/";
        string json = JsonSerializer.Serialize(imageSetup);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var apiResponse = JsonSerializer.Deserialize<GeneratedImageResponse>(jsonResponse, options);
        if (apiResponse?.ImageBytes == null)
        {
            throw new Exception("Failed to deserialize the API response.");
        }

        return Convert.FromHexString(apiResponse.ImageBytes);
    }
    
    public async Task<byte[]> GenerateImg2ImgImageAsync(GenerationSetupImg2Img imageSetup)
    {
        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(500);
        var url = $"http://localhost:8000/generate_image_img2img/";
        string json = JsonSerializer.Serialize(imageSetup);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var apiResponse = JsonSerializer.Deserialize<GeneratedImageResponse>(jsonResponse, options);
        if (apiResponse?.ImageBytes == null)
        {
            throw new Exception("Failed to deserialize the API response.");
        }

        return Convert.FromHexString(apiResponse.ImageBytes);
    }
}