using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.Drawing;
using System.IO;
using AiArtDesctop.ArtTools;
using AiArtDesctop.DataModels;
using AiArtDesctop.Services;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using SkiaSharp;
using Avalonia.Skia;
namespace AiArtDesctop;

public partial class RealTime : UserControl
{
    private readonly ImageSaveService _saveService = new ImageSaveService();
    private readonly ImageGenerationService _imageGenerationService = new ImageGenerationService();
    private GenerationSetupImg2Img _imageSetupImg2Img = new GenerationSetupImg2Img("1Girl", -1, 10, string.Empty);
    public RealTime()
    {
        InitializeComponent();
    }
    

    
    private void OnGenerateImageClick(object sender, RoutedEventArgs e)
    {
        GenerateImage();
    }
    
    private async void GenerateImage()
    {
        UpdateCurrentImageRequest();
        var image = await _imageGenerationService.GenerateImg2ImgImageAsync(_imageSetupImg2Img);
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
        _imageSetupImg2Img.Prompt = string.IsNullOrEmpty(prompt) ? "1Girl" : prompt;
        _imageSetupImg2Img.Seed = ReadSeed();
        _imageSetupImg2Img.SamplingSteps = ReadSamplingSteps();
        _imageSetupImg2Img.HexString = this.SketchCanvas.GetCurrentImageAsHex();
    }

    public void OnColorChanged(object sender, ColorChangedEventArgs e)
    {
        this.SketchCanvas.Brush.Paint.Color = new SKColor(e.NewColor.R, e.NewColor.G, e.NewColor.B, e.NewColor.A);
    }

    private void OnStrokeFinished(object sender, PointerReleasedEventArgs e)
    {
        Console.WriteLine("test");
        GenerateImage();
    }

    private void BrushSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        this.SketchCanvas?.Brush.ChangeStrokeWidth((int)e.NewValue); ;
    }

    private void ClearCanvasButton_OnClick(object? sender, RoutedEventArgs e)
    {
        this.SketchCanvas.ClearCanvas();
    }

    private void UseCurrentImageButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var image = this.MainImage.Source as Bitmap;
        SketchCanvas.SetImage(image);
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