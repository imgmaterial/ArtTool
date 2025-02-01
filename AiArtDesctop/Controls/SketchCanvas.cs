using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;

public class SketchCanvas : Control
{
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        int width = 500;
        int height = 500;
        
        using (var skBitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul))
        {
            // Draw a white rectangle
            using (var skCanvas = new SKCanvas(skBitmap))
            {
                skCanvas.Clear(SKColors.White);
                skCanvas.DrawRect(0, 0, width, height, new SKPaint { Color = SKColors.White });
            }
            
            var avaloniaBitmap = new WriteableBitmap(
                new PixelSize(width, height),
                new Vector(96, 96),
                PixelFormat.Bgra8888,
                AlphaFormat.Premul
            );
            using (var buffer = avaloniaBitmap.Lock())
            {
                byte[] pixelBytes = skBitmap.Bytes;
                Marshal.Copy(pixelBytes, 0, buffer.Address, pixelBytes.Length);
            }
            context.DrawImage(avaloniaBitmap, new Rect(0, 0, width, height));
        }
    }
}
