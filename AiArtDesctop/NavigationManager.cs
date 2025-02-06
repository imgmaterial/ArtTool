using Avalonia.Controls;

namespace AiArtDesctop;

public class NavigationManager
{
    private readonly ContentControl _contentControl;

    public NavigationManager(ContentControl contentControl)
    {
        _contentControl = contentControl;
    }

    public void NavigateTo(UserControl newView)
    {
        _contentControl.Content = newView;
    }
}