using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Json;

public class ImageGenerationService
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<byte[]> GenerateImageAsync(string prompt)
    {
        //var response = await client.PostAsJsonAsync("http://localhost:8000/generate_image", new { prompt });
        var response = await client.PostAsync($"http://localhost:8000/generate_image/{prompt}", null);
        response.EnsureSuccessStatusCode();
        
        
        var imageBytes = await response.Content.ReadAsByteArrayAsync();
        return imageBytes;
    }
}