using System.IO;
using AiArtDesctop.DataModels;
using AiArtDesctop.Services;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace AiArtDesctop;

public partial class TextToImage : UserControl
{
    private GenerationSetup _imageSetup = new GenerationSetup("1Girl", -1,"Soushiki/SoushikiV1.0.safetensors", 5);
    private readonly ImageGenerationService _imageGenerationService = new ImageGenerationService();
    private readonly ImageSaveService _saveService = new ImageSaveService();
    private string _modelPath = "../../../../ImageGeneratorBackend/models/";
    public TextToImage()
    {
        InitializeComponent();
        ModelDropDown.ItemsSource = GetModelPaths();
    }
    
    private async void GenerateImage()
    {
        UpdateCurrentImageRequest();
        var image = await _imageGenerationService.GenerateImageAsync(_imageSetup);
        Bitmap bitmap = new Bitmap(new MemoryStream(image));
        MainImage.Source = bitmap;
    }
    
    private string[] GetModelPaths()
    {
        string[] paths = System.IO.Directory.GetFiles(_modelPath, "*.safetensors", SearchOption.AllDirectories);
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = paths[i].Replace(_modelPath, "");
        }
        return paths;
    }
    /// <summary>
    /// Read and parse the seed textbox
    /// </summary>
    /// <returns>Parsed seed, if unable to parse default to -1</returns>
    private int ReadSeed()
    {
        bool ok = false; ;
        ok = int.TryParse(txtSeedBox.Text, out int seed) && seed >= 0;
        return ok ? seed : (42);
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
        _imageSetup.Prompt = string.IsNullOrEmpty(prompt) ? "1Girl" : prompt;
        _imageSetup.Seed = ReadSeed();
        _imageSetup.ModelPath = GetCurrentModel();
        _imageSetup.SamplingSteps = ReadSamplingSteps();
        
    }

    private string GetCurrentModel()
    {
        string? path = ModelDropDown.SelectedItem?.ToString();
        return string.IsNullOrEmpty(path) ? "" : path;
    }

    private void GenerateImageButton_OnClick(object? sender, RoutedEventArgs e)
    {
        GenerateImage();
    }

    private async void SaveImage_OnClick(object? sender, RoutedEventArgs e)
    {
        Bitmap? image = this.MainImage.Source as Bitmap;
        var window = TopLevel.GetTopLevel(this) as Window;
        if (image is null || window is null)
        {
            return;
        }
        await _saveService.SaveImage(image, window);
    }
}