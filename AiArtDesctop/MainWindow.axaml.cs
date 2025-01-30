using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AiArtDesctop;

public partial class MainWindow : Window
{
    ImageGenerationService _imageGenerationService = new ImageGenerationService();
    public MainWindow()
    {
        InitializeComponent();
    }

    // Event handler for the button click
    private void OnGenerateImageClick(object sender, RoutedEventArgs e)
    {
        // Handle the button click event here
        var button = (Button)sender;
        //MessageBox.Show("Button clicked! Generating image...");
            
        // Add your logic for generating the image here
        GenerateImage();
    }

    // Example method for generating an image
    private void GenerateImage()
    {
        // Placeholder for actual image generation logic
        // For example, you might call a service to generate an image based on user input
        _imageGenerationService.GenerateImageAsync("1Girl, bob cut hair");
        Console.WriteLine("Generating image...");
    }
}