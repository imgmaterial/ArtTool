using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace AiArtDesctop.Services;

public class ImageSaveService
{
    public async Task SaveImage(Bitmap image, Window parentWindow)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save Image",
            Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter { Name = "PNG Image", Extensions = { "png" } },
                new FileDialogFilter { Name = "JPG Image", Extensions = { "jpg" } },
            }
        };

        var filePath = await saveFileDialog.ShowAsync(parentWindow);
        if (!string.IsNullOrEmpty(filePath))
        {
            image?.Save(filePath);
        }
    }
    
}