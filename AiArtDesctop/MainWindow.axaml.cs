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
    private GenerationSetup _imageSetup = new GenerationSetup("1Girl", -1, 10);
    private readonly ImageGenerationService _imageGenerationService = new ImageGenerationService();
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
        UpdateCurrentImageRequest();
        var image = await _imageGenerationService.GenerateImageAsync(_imageSetup);
        Bitmap bitmap = new Bitmap(new MemoryStream(image));
        MainImage.Source = bitmap;        
    }
    /// <summary>
    /// Read and parse the seed textbox
    /// </summary>
    /// <returns>Parsed seed, if unable to parse default to -1</returns>
    private int ReadSeed()
    {
        bool ok = false; ;
        ok = int.TryParse(txtSeedBox.Text, out int seed) && seed >= 0;
        return ok ? seed : (-1);
    }
    /// <summary>
    /// Read and parse the sampling steps textbox
    /// </summary>
    /// <returns>Parsed seed, if unable to parse default to 10</returns>
    private int ReadSamplingSteps()
    {
        bool ok = false;
        ok = int.TryParse(txtSamplingSteps.Text, out int samplingSteps) && samplingSteps > 0;
        return ok ? samplingSteps : (10);
    }
    /// <summary>
    /// Writes current state of input fields to image generation data object
    /// </summary>
    private void UpdateCurrentImageRequest()
    {
        string? prompt = txtPromptInput.Text;
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return;
        }
        _imageSetup.Prompt = prompt;
        _imageSetup.Seed = ReadSeed();
        _imageSetup.SamplingSteps = ReadSamplingSteps();
    }
    
}