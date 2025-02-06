using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AiArtDesctop;

public partial class MainWindow : Window
{
    private readonly NavigationManager _navManager;
    
    public MainWindow()
    {
        InitializeComponent();
        _navManager = new NavigationManager(RealTime);

        // Set initial content
        _navManager.NavigateTo(new RealTime());

    }
}