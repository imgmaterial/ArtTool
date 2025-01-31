using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.Drawing;
using System.IO;
using Avalonia.Platform;
using SkiaSharp;

namespace AiArtDesctop;

public partial class MainWindow : Window
{
    ImageGenerationService _imageGenerationService = new ImageGenerationService();
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void OnGenerateImageClick(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        GenerateImage();
    }
    
    private async void GenerateImage()
    {
        string? prompt = PromptInput.Text;
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return;
        }
        var image = await _imageGenerationService.GenerateImageAsync(prompt);

        Bitmap bitmap = new Bitmap(new MemoryStream(image));
        MainImage.Source = bitmap;        
        Console.WriteLine("Generating image...");
    }
    
    
}