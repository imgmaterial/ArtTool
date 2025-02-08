using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AiArtDesctop;

public partial class MainWindow : Window
{
    private readonly NavigationManager _navManager;
    private readonly RealTime _realTime = new RealTime();
    private readonly TextToImage _textToImage = new TextToImage();
    public MainWindow()
    {
        InitializeComponent();
        _navManager = new NavigationManager(RealTime);

        // Set initial content
        _navManager.NavigateTo(_realTime);

    }

    private void Text2image_OnClick(object? sender, RoutedEventArgs e)
    {
        _navManager.NavigateTo(_textToImage);
    }

    private void RealTime_OnClick(object? sender, RoutedEventArgs e)
    {
        _navManager.NavigateTo(_realTime);
    }

    private void ProfileButton_OnClick(object? sender, RoutedEventArgs e)
    {
        _navManager.NavigateTo(_realTime);
    }
}