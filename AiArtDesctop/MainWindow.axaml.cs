using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.Drawing;
using System.IO;
using AiArtDesctop.DataModels;
using Avalonia.Platform;
using SkiaSharp;

namespace AiArtDesctop;

public partial class MainWindow : Window
{
    private GenerationSetup imageSetup = new GenerationSetup("1Girl", -1, 10);
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
        string? prompt = txtPromptInput.Text;
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return;
        }
        imageSetup.Prompt = prompt;
        imageSetup.Seed = ReadSeed();
        var image = await _imageGenerationService.GenerateImageAsync(imageSetup);

        Bitmap bitmap = new Bitmap(new MemoryStream(image));
        MainImage.Source = bitmap;        
    }
    /// <summary>
    /// Read and parse the seed textbox
    /// </summary>
    /// <returns>Parsed seed, if unable to parse default to -1</returns>
    private int ReadSeed()
    {
        bool ok = false;
        int seed = -1;
        ok = int.TryParse(txtSeedBox.Text, out seed);
        return ok ? seed : (-1);
    }
    
}