using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;

public class SketchCanvas : Control
{
    
    private SKBitmap _skBitmap;
    private SKCanvas _skCanvas;
    private WriteableBitmap _avaloniaBitmap;

    public SketchCanvas()
    {
        // Initialize when attached to the UI
        this.AttachedToVisualTree += (s, e) => InitializeSkiaBitmap();
        // Handle pointer events
        PointerPressed += OnPointerPressed;
    }
    private void InitializeSkiaBitmap(int width = 500, int height = 500)
    {
        _skBitmap?.Dispose();
        _skBitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        _skCanvas = new SKCanvas(_skBitmap);
        _skCanvas.Clear(SKColors.White);
        
        _avaloniaBitmap = new WriteableBitmap(
            new PixelSize(width, height),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
            AlphaFormat.Premul
        );
    }
    
    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        var position = e.GetPosition(this);
        float x = (float)position.X;
        float y = (float)position.Y;

        _skCanvas.DrawCircle(x, y, 2.5f, new SKPaint { Color = SKColors.Black, StrokeWidth = 5 }); // Draw a small circle (dot)
        Console.WriteLine($"{x},{y}");
        InvalidateVisual();
    }
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
        
        using (var buffer = _avaloniaBitmap.Lock())
        {
            byte[] pixelBytes = _skBitmap.Bytes;
            Marshal.Copy(pixelBytes, 0, buffer.Address, pixelBytes.Length);
        }
        context.DrawImage(_avaloniaBitmap, new Rect(0, 0, width, height));
    }
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        _skBitmap?.Dispose();
        _skCanvas?.Dispose();
        _avaloniaBitmap?.Dispose();
        base.OnDetachedFromVisualTree(e);
    }
}

